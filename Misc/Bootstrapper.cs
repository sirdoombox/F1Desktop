using F1Desktop.Features.Base;
using F1Desktop.Features.Root;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.Config;
using F1Desktop.Services;
using F1Desktop.Services.Interfaces;
using F1Desktop.Services.Rss;
using FluentScheduler;
using H.NotifyIcon;
using Stylet;
using StyletIoC;

namespace F1Desktop.Misc;

public class Bootstrapper : Bootstrapper<RootViewModel>
{
    private TaskbarIcon _icon;

    protected override void OnLaunch()
    {
        _icon = Application.MainWindow.GetChildOfType<TaskbarIcon>();
    }

    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        var localDataService = new LocalDataService();
        builder.Bind<IConfigService>().ToInstance(localDataService);
        builder.Bind<IDataCacheService>().ToInstance(localDataService);

        builder.Bind<TaskbarIcon>().ToFactory(_ => _icon);
        
        builder.Bind<ErgastAPIService>().ToSelf().InSingletonScope();
        builder.Bind<NewsRssService>().ToSelf().InSingletonScope();
        builder.Bind<NotificationService>().ToSelf().InSingletonScope();
        builder.Bind<ThemeService>().ToSelf().InSingletonScope();
        builder.Bind<DataResourceService>().ToSelf().InSingletonScope();
        builder.Bind<ConfigService>().ToSelf().InSingletonScope();

        builder.Bind<GlobalConfigService>().ToFactory(x =>
        {
            var cfg = new GlobalConfigService(x.Get<LocalDataService>());
            cfg.LoadConfig().GetAwaiter().GetResult();
            return cfg;
        }).InSingletonScope();
        
        builder.Bind<FeatureBase>().ToAllImplementations();
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        _icon.Dispose();
        JobManager.Stop();
        base.Dispose();
    }
}