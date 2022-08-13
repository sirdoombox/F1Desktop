using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
using F1Desktop.Services;
using JetBrains.Annotations;
using Stylet;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class WindowViewModel : Conductor<IScreen>
{
    private FeatureBase _activeViewModel;
    public FeatureBase ActiveViewModel
    {
        get => _activeViewModel;
        set
        {
            _activeViewModel?.HideFeature();
            SetAndNotify(ref _activeViewModel, value);
            _activeViewModel.ShowFeature();
        }
    }
    
    private double _userWidth;
    public double UserWidth
    {
        get => _userWidth;
        set
        {
            if (UserState == WindowState.Maximized) return;
            SetAndNotifyWithConfig(ref _userWidth, x => x.Width, value);
        }
    }

    private double _userHeight;
    public double UserHeight
    {
        get => _userHeight;
        set
        {
            if (UserState == WindowState.Maximized) return;
            SetAndNotifyWithConfig(ref _userHeight, x => x.Height, value);
        }
    }

    private double _userLeft;
    public double UserLeft
    {
        get => _userLeft;
        set
        {
            if (UserState == WindowState.Maximized) return;
            SetAndNotifyWithConfig(ref _userLeft, x => x.Left, value);
        }
    }

    private double _userTop;
    public double UserTop
    {
        get => _userTop;
        set
        {
            if (UserState == WindowState.Maximized) return;
            SetAndNotifyWithConfig(ref _userTop, x => x.Top, value);
        }
    }

    private WindowState _userState;
    public WindowState UserState
    {
        get => _userState;
        set => SetAndNotifyWithConfig(ref _userState, x => x.State, value);
    }

    public BindableCollection<FeatureBase> Features { get; } = new();

    private readonly ConfigService _cfgService;
    private GlobalConfig _globalCfg;
    
    public WindowViewModel(IEnumerable<FeatureBase> features, ConfigService cfgService, GlobalConfig globalCfg)
    {
        _cfgService = cfgService;
        _globalCfg = globalCfg;
        
        UserWidth = _globalCfg.Width;
        UserHeight = _globalCfg.Height;
        UserLeft = _globalCfg.Left;
        UserTop = _globalCfg.Top;
        UserState = _globalCfg.State;
        
        foreach (var feature in features.OrderBy(x => x.Order))
        {
            ScreenExtensions.TryActivate(feature);
            Features.Add(feature);
        }
    }

    private void SetAndNotifyWithConfig<T1>(ref T1 field, Expression<Func<GlobalConfig,T1>> propExpr, T1 value)
    {
        var hasChanged = SetAndNotify(ref field, value);
        if (!hasChanged) return;
        var expr = (MemberExpression) propExpr.Body;
        var prop = (PropertyInfo) expr.Member;
        prop.SetValue(_globalCfg, value, null);
    }

    protected override async void OnClose()
    {
        await _cfgService.WriteConfigAsync<GlobalConfig>();
        ActiveViewModel.HideFeature();
    }
}