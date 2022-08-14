using System.Threading.Tasks;
using F1Desktop.Models.Base;
using F1Desktop.Services.Interfaces;

namespace F1Desktop.Services.Local;

public class ConfigService
{
    private readonly IConfigService _configData;
    private readonly Dictionary<Type, object> _cachedConfigs = new();
    
    public ConfigService(IConfigService configData)
    {
        _configData = configData;
    }

    public async Task<T> GetConfigAsync<T>() where T : ConfigBase, new()
    {
        if (_cachedConfigs.TryGetValue(typeof(T), out var res))
            return (T)res;
        var loaded = await _configData.TryGetConfigAsync<T>();
        loaded ??= new T();
        _cachedConfigs.Add(typeof(T), loaded);
        return loaded;
    }

    public async Task WriteConfigAsync<T>() where T : ConfigBase, new()
    {
        if (!_cachedConfigs.TryGetValue(typeof(T), out var res))
            throw new InvalidOperationException($"Config of type {typeof(T).Name} has not been loaded");
        await _configData.WriteConfigToDisk((T)res);
    }
}