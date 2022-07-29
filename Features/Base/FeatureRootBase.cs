using F1Desktop.Models.Base;
using F1Desktop.Services;
using JetBrains.Annotations;

namespace F1Desktop.Features.Base;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class FeatureRootBase<T> : FeatureWindowBase where T : ConfigBase, new()
{
    protected T Config { get; private set; }
    private readonly ConfigService _configService;

    public FeatureRootBase(ConfigService configService)
    {
        _configService = configService;
    }

    protected sealed override async void OnInitialActivate()
    {
        Config = await _configService.GetConfigAsync<T>();
        OnConfigLoaded();
        OnActivationComplete();
    }

    protected override async void OnClose()
    {
        await _configService.WriteConfigAsync<T>();
    }
    
    protected virtual void OnConfigLoaded() {}
    protected virtual void OnActivationComplete() {}
}