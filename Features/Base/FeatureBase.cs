using Stylet;

namespace F1Desktop.Features.Base;

public abstract class FeatureBase : Screen
{
    protected FeatureBase(string displayName)
    {
        DisplayName = displayName;
    }
}