using System.Windows;
using F1Desktop.Features.Base;
using F1Desktop.Services.Local;
using JetBrains.Annotations;
using Stylet;

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

    public BindableCollection<FeatureBase> Features { get; } = new();

    private GlobalConfigService _globalCfg;
    
    public WindowViewModel(IEnumerable<FeatureBase> features, GlobalConfigService globalCfg, ViewManager viewManager)
    {
        _globalCfg = globalCfg;
        OnGlobalConfigChanged(null);
        _globalCfg.OnPropertyChanged += OnGlobalConfigChanged;
        
        foreach (var feature in features.OrderBy(x => x.Order))
        {
            //ScreenExtensions.TryActivate(feature);
            viewManager.CreateAndBindViewForModelIfNecessary(feature);
            var tmp = ((FrameworkElement)feature.View).ApplyTemplate();
            Features.Add(feature);
        }
    }

    private void OnGlobalConfigChanged(string propName)
    {
        SetAndNotify(ref _userWidth, _globalCfg.Width, nameof(UserWidth));
        SetAndNotify(ref _userHeight, _globalCfg.Height, nameof(UserHeight));
        SetAndNotify(ref _userLeft, _globalCfg.Left, nameof(UserLeft));
        SetAndNotify(ref _userTop, _globalCfg.Top, nameof(UserTop));
        SetAndNotify(ref _userState, _globalCfg.State, nameof(UserState));
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