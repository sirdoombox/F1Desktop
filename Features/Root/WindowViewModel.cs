using System.Threading.Tasks;
using System.Windows;
using F1Desktop.Features.Base;
using F1Desktop.Features.Settings;
using F1Desktop.Services.Interfaces;
using F1Desktop.Services.Local;
using JetBrains.Annotations;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class WindowViewModel : Conductor<IScreen>
{
    private FeatureBase _activeFeature;
    public FeatureBase ActiveFeature
    {
        get => _activeFeature;
        set
        {
            _activeFeature?.HideFeature();
            SetAndNotify(ref _activeFeature, value);
            if(_activeFeature is not SettingsRootViewModel)
                _globalCfg.LastOpenedFeature = _activeFeature.GetType();
            _activeFeature.ShowFeature();
        }
    }

    private double _userWidth;
    public double UserWidth
    {
        get => _userWidth;
        set
        {
            if (UserState == WindowState.Maximized) return;
            if (!SetAndNotify(ref _userWidth, value)) return;
            _globalCfg.Width = _userWidth;
        }
    }

    private double _userHeight;
    public double UserHeight
    {
        get => _userHeight;
        set
        {
            if (UserState == WindowState.Maximized) return;
            if (!SetAndNotify(ref _userHeight, value)) return;
            _globalCfg.Height = _userHeight;
        }
    }

    private double _userLeft;
    public double UserLeft
    {
        get => _userLeft;
        set
        {
            if (UserState == WindowState.Maximized) return;
            if (!SetAndNotify(ref _userLeft, value)) return;
            _globalCfg.Left = _userLeft;
        }
    }

    private double _userTop;
    public double UserTop
    {
        get => _userTop;
        set
        {
            if (UserState == WindowState.Maximized) return;
            if (!SetAndNotify(ref _userTop, value)) return;
            _globalCfg.Top = _userTop;
        }
    }

    private WindowState _userState;
    public WindowState UserState
    {
        get => _userState;
        set
        {
            if (!SetAndNotify(ref _userState, value)) return;
            _globalCfg.State = _userState;
        }
    }

    private bool _updateAvailable;
    public bool UpdateAvailable
    {
        get => _updateAvailable;
        set => SetAndNotify(ref _updateAvailable, value);
    }

    private string _updateVersion;
    public string UpdateVersion
    {
        get => _updateVersion;
        set => SetAndNotify(ref _updateVersion, value);
    }

    public BindableCollection<FeatureBase> Features { get; } = new();

    private GlobalConfigService _globalCfg;
    private UpdateService _update;

    public WindowViewModel(IEnumerable<FeatureBase> features,
        GlobalConfigService globalCfg,
        IViewManager viewManager,
        UpdateService update,
        INotificationService notification)
    {
        _globalCfg = globalCfg;
        OnGlobalConfigChanged(null);
        _globalCfg.OnPropertyChanged += OnGlobalConfigChanged;
        _update = update;
        _update.OnUpdateAvailable += v =>
        {
            UpdateAvailable = true;
            UpdateVersion = v;
            notification.ShowNotification($"Update Version {v} Available",
                $"Update is ready to install, click here to restart now.",
                ApplyUpdate);
        };

        foreach (var feature in features.OrderBy(x => x.Order))
        {
            viewManager.CreateAndBindViewForModelIfNecessary(feature);
            Features.Add(feature);
        }
    }

    public void ApplyUpdate()
    {
        _update.ApplyUpdate();
    }

    private void OnGlobalConfigChanged(string propName)
    {
        UserWidth = _globalCfg.Width;
        UserHeight = _globalCfg.Height;
        UserLeft = _globalCfg.Left;
        UserTop = _globalCfg.Top;
        UserState = _globalCfg.State;
    }

    public void OpenFeature(Type feature = null)
    {
        ActiveFeature = feature is null
            ? Features.First()
            : Features.First(x => x.GetType() == feature);
    }

    protected override async void OnClose()
    {
        await _globalCfg.SaveConfig();
        ActiveFeature.HideFeature();
    }
}