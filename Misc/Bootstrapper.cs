using System;
using F1Desktop.Features.Root;
using F1Desktop.Misc.Extensions;
using F1Desktop.Services;
using F1Desktop.Services.Interfaces;
using H.NotifyIcon;
using Stylet;
using StyletIoC;

namespace F1Desktop.Misc;

public class Bootstrapper : Bootstrapper<RootViewModel>
{
    private TaskbarIcon _icon;

    protected override void OnLaunch() => _icon = Application.MainWindow.GetChildOfType<TaskbarIcon>();

    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        var localDataService = new LocalDataService();
        builder.Bind<IConfigService>().ToInstance(localDataService);
        builder.Bind<IDataCacheService>().ToInstance(localDataService);
        builder.Bind<ErgastAPIService>().ToSelf().InSingletonScope();
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        _icon.Dispose();
        base.Dispose();
    }
}