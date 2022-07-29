using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using F1Desktop.Attributes;
using F1Desktop.Misc;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.Base;
using F1Desktop.Services.Interfaces;

namespace F1Desktop.Services;

public class LocalDataService : IDataCacheService, IConfigService
{
    private readonly string _cachePath;
    private readonly string _configPath;
    private static readonly JsonSerializerOptions JsonOptions;

    static LocalDataService()
    {
        JsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.General);
    }
    
    public LocalDataService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appDataPath = Path.Combine(appData, Constants.AppName);
        _cachePath = Path.Combine(appDataPath, "Cache");
        _configPath = Path.Combine(appDataPath, "Data");
        Directory.CreateDirectory(_cachePath);
        Directory.CreateDirectory(_configPath);
    }

    public async Task<(T, bool)> TryGetCacheAsync<T>() where T : CachedDataBase
    {
        var data = await TryReadDataFromFile<T>(_cachePath);
        if (data is null) return (null, false);
        var attrib = typeof(T).GetAttribute<CacheDurationAttribute>();
        var validUntil = data.CacheTime + attrib.CacheValidFor;
        return (data, validUntil >= DateTimeOffset.Now);
    }

    public async Task WriteCacheToDisk<T>(T cache) where T : CachedDataBase
    {
        if (cache is null) return;
        cache.CacheTime = DateTimeOffset.Now;
        await WriteDataToFile(_cachePath, cache);
    }

    public Task<T> TryGetConfigAsync<T>() where T : ConfigBase => 
        TryReadDataFromFile<T>(_configPath);

    public Task WriteConfigToDisk<T>(T config) where T : ConfigBase =>
        WriteDataToFile(_configPath, config);

    private static async Task<T> TryReadDataFromFile<T>(string basePath) where T : class
    {
        var filepath = GetFilePath<T>(basePath);
        if (!File.Exists(filepath)) return null;
        await using var filestream = File.Open(filepath, FileMode.Open);
        return await JsonSerializer.DeserializeAsync<T>(filestream);
    }

    private static async Task WriteDataToFile<T>(string basePath, T data, bool writeIndented = false) where T : class
    {
        var filepath = GetFilePath<T>(basePath);
        await using var filestream = File.Create(filepath);
        JsonOptions.WriteIndented = writeIndented;
        await JsonSerializer.SerializeAsync(filestream, data, JsonOptions);
    }

    private static string GetFilePath<T>(string basePath)
    {
        var type = typeof(T);
        var attribs = type.GetCustomAttributes(typeof(FilenameAttribute), false);
        var filename = attribs.Length > 0 ? ((FilenameAttribute)attribs[0]).Filename : type.Name;
        return Path.Combine(basePath, filename);
    }
}