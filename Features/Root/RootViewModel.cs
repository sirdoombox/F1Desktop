using System.Windows;
using F1Desktop.Features.Base;
using F1Desktop.Features.Calendar;
using F1Desktop.Features.News;
using JetBrains.Annotations;
using Stylet;
using Screen = Stylet.Screen;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class RootViewModel : Conductor<IScreen>.Collection.OneActive
{
    private readonly IWindowManager _wm;

    public RootViewModel(IWindowManager wm, IEnumerable<FeatureBase> featureBases)
    {
        Items.AddRange(featureBases);
        _wm = wm;
    }

    public void OpenWindow(Type toOpen)
    {
        _wm.ShowWindow(this);
        ActivateItem(Items.First(x => x.GetType() == toOpen));
    }

    public void Exit() => Application.Current.Shutdown();
}