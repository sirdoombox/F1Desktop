using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using F1Desktop.Attributes;
using F1Desktop.Misc;
using F1Desktop.Misc.JsonConverters;
using F1Desktop.Models.Base;
using F1Desktop.Services.Base;
using F1Desktop.Services.Interfaces;

namespace F1Desktop.Services.Local;

public class LocalDataService : ServiceBase, IConfigService
{
    private static readonly JsonSerializerOptions IndentedJsonOptions;
    private static readonly JsonSerializerOptions DefaultJsonOptions;
    
    static LocalDataService()
    {
        IndentedJsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.General)
        {
            WriteIndented = true,
            Converters = { new TypeJsonConverter() }
        };
        DefaultJsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.General)
        {
            Converters = { new TypeJsonConverter() }
        };
    }

    public Action OnGlobalConfigReset { get; set; }
    
    private readonly Dictionary<Type, object> _cachedConfigs = new();

    public LocalDataService()
    {
        Directory.CreateDirectory(Constants.App.ConfigPath);
    }

    public async Task<T> GetConfigAsync<T>() where T : ConfigBase, new()
    {
        if (_cachedConfigs.TryGetValue(typeof(T), out var res))
            return (T)res;
        var loaded = await TryReadDataFromFile<T>(Constants.App.ConfigPath, true);
        loaded ??= new T();
        _cachedConfigs.Add(typeof(T), loaded);
        return loaded;
    }

    public async Task WriteConfigToDiskAsync<T>() where T : ConfigBase
    {
        if (!_cachedConfigs.TryGetValue(typeof(T), out var res))
            throw new InvalidOperationException($"Config of type {typeof(T).Name} has not been loaded");
        await WriteDataToFile(Constants.App.ConfigPath, (T)res, true);
    }

    private static async Task<T> TryReadDataFromFile<T>(string basePath, bool readIndented = false) where T : class
    {
        var filepath = GetFilePath<T>(basePath);
        if (!File.Exists(filepath)) return null;
        await using var filestream = File.Open(filepath, FileMode.Open);
        return await JsonSerializer.DeserializeAsync<T>(filestream,
            readIndented ? IndentedJsonOptions : DefaultJsonOptions);
    }

    private static async Task WriteDataToFile<T>(string basePath, T data, bool writeIndented = false) where T : class
    {
        var filepath = GetFilePath<T>(basePath);
        await using var filestream = File.Create(filepath);
        await JsonSerializer.SerializeAsync(filestream, data, writeIndented ? IndentedJsonOptions : DefaultJsonOptions);
    }

    private static string GetFilePath<T>(string basePath)
    {
        var type = typeof(T);
        var attribs = type.GetCustomAttributes(typeof(FilenameAttribute), false);
        var filename = attribs.Length > 0 ? ((FilenameAttribute)attribs[0]).Filename : type.Name;
        return Path.Combine(basePath, filename);
    }
}