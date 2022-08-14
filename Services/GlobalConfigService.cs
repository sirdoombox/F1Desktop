using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using F1Desktop.Models.Config;

namespace F1Desktop.Services;

public class GlobalConfigService
{
    public Action<string> OnPropertyChanged { get; set; }
    
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
    
    private readonly LocalDataService _localData;
    private GlobalConfig _config;
    
    public GlobalConfigService(LocalDataService localData)
    {
        _localData = localData;
    }

    private void SetAndNotify<T>(ref T field, Expression<Func<GlobalConfig,T>> propExpr, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        var expr = (MemberExpression) propExpr.Body;
        var prop = (PropertyInfo) expr.Member;
        prop.SetValue(_config, value, null);
        OnPropertyChanged?.Invoke(propertyName);
    }

    public void ResetDefault() => _config = new GlobalConfig();

    public async Task LoadConfig()
    {
        _config ??= await _localData.TryGetConfigAsync<GlobalConfig>();
        UseLightTheme = _config.LightTheme;
        Use24HourClock = _config.Use24HourClock;
    }

    public async Task SaveConfig()
    {
        if (_config is null) throw new InvalidOperationException("Global config isn't loaded.");
        await _localData.WriteConfigToDisk(_config);
    }
}