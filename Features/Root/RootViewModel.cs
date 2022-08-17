using System.Windows;
using F1Desktop.Services.Local;
using JetBrains.Annotations;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class RootViewModel : Conductor<IScreen>.Collection.AllActive
{
    private readonly IWindowManager _wm;
    private readonly WindowViewModel _window;
    private readonly FirstRunWindowViewModel _firstRunWindow;
    
    private readonly GlobalConfigService _cfg;
    private readonly UpdateService _update;

    public RootViewModel(IWindowManager wm, WindowViewModel window, FirstRunWindowViewModel firstRunWindow,
        GlobalConfigService cfg, UpdateService update)
    {
        _wm = wm;
        _window = window;
        _update = update;
        _firstRunWindow = firstRunWindow;
        _cfg = cfg;
    }

    protected override void OnInitialActivate()
    {
        if (!_update.FirstRun) return;
        _wm.ShowWindow(_firstRunWindow);
        ((Window)_firstRunWindow.View).Activate();
    }

    public void OpenWindow(Type toOpen)
    {
        _wm.ShowWindow(_window);
        ((Window)_window.View).Activate();
        _window.OpenFeature(toOpen);
    }

    public void OpenDefault()
    {
        _wm.ShowWindow(_window);
        ((Window)_window.View).Activate();
        _window.OpenFeature(_cfg.LastOpenedFeature);
    }

    public void Exit() => Application.Current.Shutdown();
}