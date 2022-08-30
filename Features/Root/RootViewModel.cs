using System.Windows;
using F1Desktop.Misc.Extensions;
using F1Desktop.Services.Interfaces;
using F1Desktop.Services.Local;
using JetBrains.Annotations;
using Serilog;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class RootViewModel : Conductor<IScreen>.Collection.AllActive
{
    private readonly IWindowManager _wm;
    private readonly WindowViewModel _window;
    private readonly FirstRunWindowViewModel _firstRunWindow;

    private readonly GlobalConfigService _cfg;
    private readonly UpdateService _update;
    private readonly INotificationService _notification;
    private readonly ILogger _logger;
    
    public RootViewModel(IWindowManager wm, WindowViewModel window, FirstRunWindowViewModel firstRunWindow,
        GlobalConfigService cfg, UpdateService update, INotificationService notification, ILogger logger)
    {
        _wm = wm;
        _window = window;
        _update = update;
        _logger = logger;
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
        if (_cfg.ShowWindowOnStartup)
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