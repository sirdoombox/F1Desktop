using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Debug.Base;

public abstract class DebugFeatureBase : Screen
{
    public PackIconMaterialKind Icon { get; }
    
    public DebugFeatureBase(string name, PackIconMaterialKind icon)
    {
        DisplayName = name;
        Icon = icon;
    }
}