using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
using F1Desktop.Services.Interfaces;
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
    
    public WindowViewModel(IEnumerable<FeatureBase> features, GlobalConfigService globalCfg)
    {
        _globalCfg = globalCfg;
        _globalCfg.OnPropertyChanged += OnGlobalConfigChanged;
        OnGlobalConfigChanged(null);
        
        foreach (var feature in features.OrderBy(x => x.Order))
        {
            ScreenExtensions.TryActivate(feature);
            Features.Add(feature);
        }
    }

    public void OnGlobalConfigChanged(string propName)
    {
        UserWidth = _globalCfg.Width;
        UserHeight = _globalCfg.Height;
        UserLeft = _globalCfg.Left;
        UserTop = _globalCfg.Top;
        UserState = _globalCfg.State;
    }

    protected override async void OnClose()
    {
        await _globalCfg.SaveConfig();
        ActiveFeature.HideFeature();
    }
}