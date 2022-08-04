using F1Desktop.Features.Root;
using F1Desktop.Misc.Extensions;
using F1Desktop.Services;
using F1Desktop.Services.Interfaces;
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
        // Sometimes notifications don't work... but they mostly seem to now so ?
        // _icon.ShowNotification("test", "test");
    }

    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        var localDataService = new LocalDataService();
        builder.Bind<IConfigService>().ToInstance(localDataService);
        builder.Bind<IDataCacheService>().ToInstance(localDataService);
        
        builder.Bind<ErgastAPIService>().ToSelf().InSingletonScope();
        builder.Bind<NewsRssService>().ToSelf().InSingletonScope();
        builder.Bind<NotificationService>().ToFactory(_ => new NotificationService(() => _icon)).InSingletonScope();
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        _icon.Dispose();
        JobManager.Stop();
        base.Dispose();
    }
}