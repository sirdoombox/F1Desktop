using F1Desktop.Features.Base;
using JetBrains.Annotations;
using Stylet;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class WindowViewModel : Conductor<IScreen>.Collection.AllActive
{
    private IScreen _activeItem;
    public IScreen ActiveItem
    {
        get => _activeItem;
        set => SetAndNotify(ref _activeItem, value);
    }
    
    public WindowViewModel(IEnumerable<FeatureBase> features)
    {
        foreach(var feature in features)
            ActivateItem(feature);
    }
}