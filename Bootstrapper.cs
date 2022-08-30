using System.Diagnostics;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using F1Desktop.Features.Base;
using F1Desktop.Features.Debug;
using F1Desktop.Features.Debug.Base;
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
    private Logger _log;
    private IAppTools _appTools;
    private SemanticVersion _version;
    private bool _firstRun;
    private bool _justUpdated;
    private bool _debug;

    public override void Start(string[] args)
    {
        if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(AppContext.BaseDirectory)).Length > 1)
            Process.GetCurrentProcess().Kill();
        
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

        _log = new LoggerConfiguration()
            .WriteTo.File(Path.Combine(Constants.App.LogsPath, "Log-.log"),
                rollingInterval: RollingInterval.Hour,
                retainedFileCountLimit: 5)
            .CreateLogger();

        SquirrelLocator.CurrentMutable.Register(() => new SquirrelLogger(_log), typeof(Squirrel.SimpleSplat.ILogger));

        if (args.Any(x => x.Contains("just-updated")))
            _justUpdated = true;
        if (args.Any(x => x.Contains("debug")))
            _debug = true;

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

    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        var localDataService = new LocalDataService();
        builder.Bind<IConfigService>().ToInstance(localDataService);

        builder.Bind<StartupState>().ToFactory(_ => new StartupState
        {
            AppTools = _appTools,
            FirstRun = _firstRun,
            JustUpdated = _justUpdated,
            Version = _version,
            Debug = _debug
        });

        builder.Bind<WindowViewModel>().ToSelf().InSingletonScope();
        builder.Bind<ErgastAPIService>().ToSelf().InSingletonScope();
        builder.Bind<NewsRssService>().ToSelf().InSingletonScope();
        builder.Bind<ThemeService>().ToSelf().InSingletonScope();
        builder.Bind<DataResourceService>().ToSelf().InSingletonScope();
        builder.Bind<GlobalConfigService>().ToSelf().InSingletonScope();
        builder.Bind<RegistryService>().ToSelf().InSingletonScope();
        builder.Bind<UpdateService>().ToSelf().InSingletonScope();
        
        builder.Bind<TimeService>().ToSelf().InSingletonScope();
        builder.Bind<ITimeService>().ToFactory(c => c.Get<TimeService>());
        builder.Bind<ITimeServiceDebug>().ToFactory(c => c.Get<TimeService>());

        builder.Bind<NotificationService>().ToSelf().InSingletonScope();
        builder.Bind<INotificationService>().ToFactory(c => c.Get<NotificationService>());
        builder.Bind<INotificationServiceDebug>().ToFactory(c => c.Get<NotificationService>());
        
        builder.Bind<Serilog.ILogger>().ToInstance(_log);
        builder.BindAllImplementers<FeatureBase>();
        builder.BindAllImplementers<SetingsCategoryViewModelBase>();
        builder.BindAllImplementers<DebugFeatureBase>();
    }

    protected override void Configure()
    {
        // ensure services are built - no type depends on them, but they react to GlobalConfigService changes.
        Container.Get<RegistryService>();
        Container.Get<ThemeService>();
    }

    protected override async void Launch()
    {
        await Container.Get<GlobalConfigService>().LoadConfig();
        await Container.Get<UpdateService>().Update();
        
        var tasks = new List<Task>();
        foreach (var feature in Container.GetAll<FeatureBase>())
            tasks.Add(feature.LoadDataInBackground());
        await Task.WhenAll(tasks);
        
        base.Launch();
        
        Container.Get<ThemeService>().PassIcon(Application.MainWindow.GetChildOfType<TaskbarIcon>());
        Container.Get<NotificationService>().PassIcon(Application.MainWindow.GetChildOfType<TaskbarIcon>());
        if (!_debug) return;
        var wm = Container.Get<IWindowManager>();
        wm.ShowWindow(Container.Get<DebugWindowViewModel>());
    }

    private bool _hasProvidedCrashFeedback;

    protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e) =>
        HandleException(e.Exception);

    private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e) =>
        HandleException((Exception)e.ExceptionObject);

    private void HandleException(Exception e)
    {
        _log.Fatal(e, "");
        if (_hasProvidedCrashFeedback) return;
        _hasProvidedCrashFeedback = true;
        Execute.OnUIThread(() => Container.Get<WindowViewModel>()?.View?.AsWindow().Hide());
        MessageBox.Show("See C:\\Users\\USERNAME\\AppData\\Roaming\\F1Desktop\\Logs for technical information.",
            "F1 Desktop Has Crashed Unexpectedly");
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        //_icon?.Dispose();
        JobManager.Stop();
        base.Dispose();
    }
}