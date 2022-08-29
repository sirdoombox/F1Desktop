using F1Desktop.Features.Debug.Base;

namespace F1Desktop.Features.Debug;

public sealed class DebugWindowViewModel : Conductor<DebugFeatureBase>.Collection.OneActive
{
    public DebugWindowViewModel(IEnumerable<DebugFeatureBase> debugFeatures)
    {
        Items.AddRange(debugFeatures);
        ActivateItem(debugFeatures.First());
    }
}