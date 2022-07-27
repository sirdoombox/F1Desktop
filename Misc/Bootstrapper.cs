using F1Desktop.Features.Root;
using F1Desktop.Services;
using F1Desktop.Services.Interfaces;
using Stylet;
using StyletIoC;

namespace F1Desktop.Misc;

public class Bootstrapper : Bootstrapper<RootViewModel>
{
    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        var localDataService = new LocalDataService();
        builder.Bind<IConfigService>().ToInstance(localDataService);
        builder.Bind<IDataCacheService>().ToInstance(localDataService);
        builder.Bind<ErgastAPIService>().ToSelf().InSingletonScope();
    }
}