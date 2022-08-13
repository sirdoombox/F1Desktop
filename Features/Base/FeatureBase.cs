using MahApps.Metro.IconPacks;
using Stylet;

namespace F1Desktop.Features.Base;

public abstract class FeatureBase : Screen
{
    public PackIconMaterialKind Icon { get; }
    public byte Order { get; }

    private bool _featureHasBeenOpened;
    
    protected FeatureBase(string displayName, PackIconMaterialKind icon, byte order = byte.MinValue)
    {
        DisplayName = displayName;
        Icon = icon;
        Order = order;
    }

    public void ShowFeature()
    {
        if (_featureHasBeenOpened) return;
        _featureHasBeenOpened = true;
        OnFeatureFirstOpened();
    }

    public void HideFeature() => OnFeatureHidden();

    protected virtual void OnFeatureHidden(){}
    
    protected virtual void OnFeatureFirstOpened(){}
}