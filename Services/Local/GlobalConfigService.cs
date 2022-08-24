using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using F1Desktop.Models.Config;
using F1Desktop.Services.Base;
using F1Desktop.Services.Interfaces;

namespace F1Desktop.Services.Local;

public class GlobalConfigService : ServiceBase
{
    public Action<string> OnPropertyChanged { get; set; }
    public Action OnConfigLoaded { get; set; }

    private bool _use24HourClock;
    public bool Use24HourClock
    {
        get => _use24HourClock;
        set => SetAndNotify(ref _use24HourClock, c => c.Use24HourClock, value);
    }

    private bool _useLightTheme;
    public bool UseLightTheme
    {
        get => _useLightTheme;
        set => SetAndNotify(ref _useLightTheme, c => c.LightTheme, value);
    }

    private double _width;
    public double Width
    {
        get => _width;
        set => SetAndNotify(ref _width, c => c.Width, value);
    }

    private double _height;
    public double Height
    {
        get => _height;
        set => SetAndNotify(ref _height, c => c.Height, value);
    }

    private double _left;
    public double Left
    {
        get => _left;
        set => SetAndNotify(ref _left, c => c.Left, value);
    }

    private double _top;
    public double Top
    {
        get => _top;
        set => SetAndNotify(ref _top, c => c.Top, value);
    }

    private WindowState _state;
    public WindowState State
    {
        get => _state;
        set => SetAndNotify(ref _state, c => c.State, value);
    }

    private Type _lastOpenedFeature;
    public Type LastOpenedFeature
    {
        get => _lastOpenedFeature;
        set => SetAndNotify(ref _lastOpenedFeature, c => c.LastOpenedFeature, value);
    }

    private bool _startWithWindows;
    public bool StartWithWindows
    {
        get => _startWithWindows;
        set => SetAndNotify(ref _startWithWindows, c => c.StartWithWindows, value);
    }
    
    private bool _showWindowOnStartup;
    public bool ShowWindowOnStartup
    {
        get => _showWindowOnStartup;
        set => SetAndNotify(ref _showWindowOnStartup, c => c.ShowWindowOnStartup, value);
    }

    private readonly IConfigService _configService;
    private GlobalConfig _config;

    public GlobalConfigService(IConfigService configService)
    {
        _configService = configService;
    }

    private void SetAndNotify<T>(ref T field, Expression<Func<GlobalConfig, T>> propExpr, T value,
        [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        var expr = (MemberExpression)propExpr.Body;
        var prop = (PropertyInfo)expr.Member;
        prop.SetValue(_config, value, null);
        OnPropertyChanged?.Invoke(propertyName);
    }

    public async Task ResetDefault()
    {
        _config.Default();
        _configService.OnGlobalConfigReset?.Invoke();
        await LoadConfig();
    }

    public async Task LoadConfig()
    {
        _config ??= await _configService.GetConfigAsync<GlobalConfig>();
        _left = _config.Left;
        _top = _config.Top;
        _width = _config.Width;
        _height = _config.Height;
        _state = _config.State;
        _useLightTheme = _config.LightTheme;
        _use24HourClock = _config.Use24HourClock;
        _lastOpenedFeature = _config.LastOpenedFeature;
        _startWithWindows = _config.StartWithWindows;
        _showWindowOnStartup = _config.ShowWindowOnStartup;
        OnPropertyChanged?.Invoke(null);
        OnConfigLoaded?.Invoke();
    }

    public async Task SaveConfig()
    {
        if (_config is null) throw new InvalidOperationException("Global config isn't loaded.");
        await _configService.WriteConfigToDiskAsync<GlobalConfig>();
    }
}