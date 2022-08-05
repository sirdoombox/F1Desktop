using F1Desktop.Models.Base;
using F1Desktop.Services;
using JetBrains.Annotations;

namespace F1Desktop.Features.Base;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class FeatureBaseWithConfig<T> : FeatureBase where T : ConfigBase, new()
{
    protected T Config { get; private set; }
    private readonly ConfigService _configService;

    protected FeatureBaseWithConfig(string displayName, ConfigService configService) : base(displayName)
    {
        DisplayName = displayName;
        _configService = configService;
    }

    protected override async void OnActivate()
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