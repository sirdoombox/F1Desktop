using System.Windows;
using F1Desktop.Features.Base;
using JetBrains.Annotations;
using Stylet;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class RootViewModel : Conductor<IScreen>.Collection.AllActive
{
    private readonly IWindowManager _wm;
    private readonly WindowViewModel _window;
    
    public RootViewModel(IWindowManager wm, WindowViewModel window)
    {
        _wm = wm;
        _window = window;
    }

    public void OpenWindow(Type toOpen)
    {
        _wm.ShowWindow(_window);
        _window.ActiveItem = _window.Items.First(x => x.GetType() == toOpen);
    }

    public void Exit() => Application.Current.Shutdown();
}