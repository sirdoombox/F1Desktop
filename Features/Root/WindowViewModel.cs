using F1Desktop.Features.Base;
using JetBrains.Annotations;
using Stylet;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class WindowViewModel : Conductor<IScreen>
{
    private IScreen _activeViewModel;
    public IScreen ActiveViewModel
    {
        get => _activeViewModel;
        set => SetAndNotify(ref _activeViewModel, value);
    }

    public BindableCollection<FeatureBase> ViewModels { get; } = new();

    public WindowViewModel(IEnumerable<FeatureBase> features)
    {
        foreach (var feature in features)
        {
            ScreenExtensions.TryActivate(feature);
            ViewModels.Add(feature);
        }
    }
}