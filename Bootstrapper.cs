using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Windows;
using System.Windows.Threading;
using F1Desktop.Features.Base;
using F1Desktop.Features.Root;
using F1Desktop.Features.Settings.Settings.Base;
using F1Desktop.Misc;
using F1Desktop.Misc.Extensions;
using F1Desktop.Misc.Logging;
using F1Desktop.Models.Misc;
using F1Desktop.Services.Interfaces;
using F1Desktop.Services.Local;
using F1Desktop.Services.Remote;
using FluentScheduler;
using H.NotifyIcon;
using NuGet.Versioning;
using Serilog;
using Serilog.Core;
using Squirrel;
using Squirrel.SimpleSplat;
using StyletIoC;
using Constants = F1Desktop.Misc.Constants;

namespace F1Desktop;

public class Bootstrapper : Bootstrapper<RootViewModel>
{
    private TaskbarIcon _icon;
    private Logger _log;
    private IAppTools _appTools;
    private SemanticVersion _version;
    private bool _firstRun;
    private bool _justUpdated;

    public override void Start(string[] args)
    {
        if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(AppContext.BaseDirectory)).Length > 1)
            Process.GetCurrentProcess().Kill();
        
        AppDomain.CurrentDomain.FirstChanceException += CurrentDomainOnFirstChanceException;

        _log = new LoggerConfiguration()
            .WriteTo.File(Path.Combine(Constants.App.LogsPath, "Log-.log"),
                rollingInterval: RollingInterval.Hour,
                retainedFileCountLimit: 5)
            .CreateLogger();

        SquirrelLocator.CurrentMutable.Register(() => new SquirrelLogger(_log), typeof(Squirrel.SimpleSplat.ILogger));

        if (args.Any(x => x.Contains("just-updated")))
            _justUpdated = true;

        SquirrelAwareApp.HandleEvents(
            onInitialInstall: (_, t) =>
                t.CreateShortcutForThisExe(ShortcutLocation.StartMenu),
            onAppUninstall: (_, t) =>
            {
                RegistryHelper.DeleteKey(Constants.Misc.RegistryStartupSubKey, Constants.App.Name);
                t.RemoveShortcutForThisExe();
                MessageBox.Show("F1 Desktop Successfully Uninstalled.", "Uninstall Complete.");
            },
            onEveryRun: (v, t, f) =>
            {
                _version = v;
                _appTools = t;
                _firstRun = f;
            });

        base.Start(args);
    }

    protected override void OnLaunch() =>
        _icon = Application.MainWindow.GetChildOfType<TaskbarIcon>();

    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        var localDataService = new LocalDataService();
        builder.Bind<IConfigService>().ToInstance(localDataService);

        builder.Bind<TaskbarIcon>().ToFactory(_ => _icon);

        builder.Bind<StartupState>().ToFactory(_ => new StartupState()
        {
            AppTools = _appTools,
            FirstRun = _firstRun,
            JustUpdated = _justUpdated,
            Version = _version
        });

        builder.Bind<WindowViewModel>().ToSelf().InSingletonScope();
        builder.Bind<ErgastAPIService>().ToSelf().InSingletonScope();
        builder.Bind<NewsRssService>().ToSelf().InSingletonScope();
        builder.Bind<NotificationService>().ToSelf().InSingletonScope();
        builder.Bind<ThemeService>().ToSelf().InSingletonScope();
        builder.Bind<DataResourceService>().ToSelf().InSingletonScope();
        builder.Bind<GlobalConfigService>().ToSelf().InSingletonScope();
        builder.Bind<RegistryService>().ToSelf().InSingletonScope();
        builder.Bind<UpdateService>().ToSelf().InSingletonScope();
        builder.Bind<Serilog.ILogger>().ToInstance(_log);
        builder.BindAllImplementers<FeatureBase>();
        builder.BindAllImplementers<SetingsCategoryViewModelBase>();
    }

    protected override async void Configure()
    {
        await Container.Get<GlobalConfigService>().LoadConfig();
        // ensure services are built - no type depends on them, but they react to GlobalConfigService changes.
        Container.Get<RegistryService>();
        Container.Get<ThemeService>();
        // Install updates
        await Container.Get<UpdateService>().Update();
    }

    private bool _hasProvidedCrashFeedback;

    private void CurrentDomainOnFirstChanceException(object sender, FirstChanceExceptionEventArgs e) =>
        HandleException(e.Exception);

    protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e) =>
        HandleException(e.Exception);

    private void HandleException(Exception e)
    {
#if DEBUG
        return; // prevent handling exceptions when debugging, it's annoying.
#endif
        _log.Fatal(e, "");
        if (_hasProvidedCrashFeedback) return;
        _hasProvidedCrashFeedback = true;
        Execute.OnUIThread(() => Container.Get<WindowViewModel>().View.AsWindow().Hide());
        Dispose();
        MessageBox.Show("See C:\\Users\\USERNAME\\AppData\\Roaming\\F1Desktop\\Logs for technical information.",
            "F1 Desktop Has Crashed Unexpectedly");
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        _icon?.Dispose();
        JobManager.Stop();
        base.Dispose();
    }
}