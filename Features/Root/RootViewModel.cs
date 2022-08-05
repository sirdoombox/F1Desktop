using System.Windows;
using F1Desktop.Features.Base;
using JetBrains.Annotations;
using Stylet;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class RootViewModel : Conductor<IScreen>.Collection.AllActive
{
    private readonly IWindowManager _wm;
    
    private IScreen _activeItem;
    public IScreen ActiveItem
    {
        get => _activeItem;
        set => SetAndNotify(ref _activeItem, value);
    }

    public RootViewModel(IWindowManager wm, IEnumerable<FeatureBase> featureBases)
    {
        foreach(var feature in featureBases)
            ActivateItem(feature);
        _wm = wm;
    }

    public void OpenWindow(Type toOpen)
    {
        _wm.ShowWindow(this);
        ActiveItem = Items.First(x => x.GetType() == toOpen);
    }

    public void Exit() => Application.Current.Shutdown();
}