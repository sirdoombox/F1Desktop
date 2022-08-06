using MahApps.Metro.IconPacks;
using Stylet;

namespace F1Desktop.Features.Base;

public abstract class FeatureBase : Screen
{
    public PackIconMaterialKind Icon { get; }
    public byte Order { get; }
    
    protected FeatureBase(string displayName, PackIconMaterialKind icon, byte order = byte.MinValue)
    {
        DisplayName = displayName;
        Icon = icon;
        Order = order;
    }

    public virtual void OnFeatureHidden(){}
}