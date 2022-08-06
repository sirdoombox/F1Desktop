using System.Threading.Tasks;
using F1Desktop.Models.Base;
using F1Desktop.Services.Interfaces;

namespace F1Desktop.Services;

public class ConfigService
{
    private readonly IConfigService _configData;
    private readonly Dictionary<Type, object> _cachedConfigs = new();
    private readonly Dictionary<Type, Action> _configUpdates = new();

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

    public void NotifyOfConfigChange<T>() where T : ConfigBase
    {
        if (!_configUpdates.TryGetValue(typeof(T), out var action)) return;
        action?.Invoke();
    }

    public void SubscribeToConfigChange<T>(Action callback) where T : ConfigBase
    {
        if (!_configUpdates.ContainsKey(typeof(T)))
            _configUpdates.Add(typeof(T), () => { });
        _configUpdates[typeof(T)] += callback;
    }

    public void UnsubscribeToConfigChange<T>(Action callback) where T : ConfigBase
    {
        if (!_configUpdates.ContainsKey(typeof(T))) return;
        _configUpdates[typeof(T)] -= callback;
    }
}