using System.Windows;
using F1Desktop.Misc.Extensions;
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
    private readonly NotificationService _notification;

    public RootViewModel(IWindowManager wm, WindowViewModel window, FirstRunWindowViewModel firstRunWindow,
        GlobalConfigService cfg, UpdateService update, NotificationService notification)
    {
        _wm = wm;
        _window = window;
        _update = update;
        _notification = notification;
        _firstRunWindow = firstRunWindow;
        _cfg = cfg;
        _cfg.OnGlobalConfigFirstLoaded += OnGlobalConfigFirstLoaded;
    }

    private void OnGlobalConfigFirstLoaded()
    {
        if (!_update.FirstRun && !_update.IsJustUpdated && _cfg.ShowWindowOnStartup) 
            OpenDefault();
    }

    protected override void OnInitialActivate()
    {
        _notification.ShowNotification("Update Installed.", 
            $"Update {_update.Version} Successfully Installed",
            OpenDefault);
        if (_update.FirstRun)
        {
            _firstRunWindow.OnFirstRunClosed += OpenDefault;
            _wm.ShowWindow(_firstRunWindow);
            _firstRunWindow.View.AsWindow().Activate();
        }
        else if (_update.IsJustUpdated)
        {
            _notification.ShowNotification("Update Installed.", 
                $"Update {_update.Version} Successfully Installed",
                OpenDefault);
        }
        else if (_cfg.ShowWindowOnStartup)
            OpenDefault();
    }

    public void OpenWindow(Type toOpen)
    {
        _wm.ShowWindow(_window);
        _window.View.AsWindow().Activate();
        _window.OpenFeature(toOpen);
    }

    public void OpenDefault()
    {
        _wm.ShowWindow(_window);
        _window.View.AsWindow().Activate();
        _window.OpenFeature(_cfg.LastOpenedFeature);
    }

    public void Exit() => Application.Current.Shutdown();
}