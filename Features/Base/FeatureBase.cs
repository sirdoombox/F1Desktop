using MahApps.Metro.IconPacks;
using Serilog;
using StyletIoC;

namespace F1Desktop.Features.Base;

public abstract class FeatureBase : Screen, IInjectionAware
{
    [Inject]
    protected ILogger Logger { get; set; }
    
    private bool _featureLoading;
    public bool FeatureLoading
    {
        get => _featureLoading;
        set => SetAndNotify(ref _featureLoading, value);
    }
    
    public PackIconMaterialKind Icon { get; }
    public byte Order { get; }

    private bool _featureHasBeenOpened;

    protected FeatureBase(string displayName, PackIconMaterialKind icon, byte order = byte.MinValue)
    {
        DisplayName = displayName;
        Icon = icon;
        Order = order;
    }

    public virtual void ShowFeature()
    {
        ScreenExtensions.TryActivate(this);
        if (_featureHasBeenOpened) return;
        _featureHasBeenOpened = true;
        OnFeatureFirstOpened();
    }

    public void HideFeature()
    {
        ScreenExtensions.TryDeactivate(this);
        OnFeatureHidden();
    }
    
    public void ParametersInjected() => OnPropertiesInjected();
    
    protected virtual void OnFeatureHidden()
    {
    }

    protected virtual void OnFeatureFirstOpened()
    {
    }

    protected virtual void OnPropertiesInjected()
    {
    }
}