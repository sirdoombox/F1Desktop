using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using F1Desktop.Attributes;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.Base;
using F1Desktop.Services.Interfaces;
using JetBrains.Annotations;

namespace F1Desktop.Services.Remote;

[UsedImplicitly]
public class ErgastAPIService
{
    private static readonly HttpClient Client;
    private static readonly JsonSerializerOptions Options;

    private readonly IDataCacheService _cacheService;

    static ErgastAPIService()
    {
        Client = new HttpClient
        {
            BaseAddress = new Uri("https://ergast.com/api/f1/")
        };
        Options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public ErgastAPIService(IDataCacheService cacheService)
    {
        _cacheService = cacheService;
    }
    
    /// <summary>
    /// Get the specified type from either the API or the local cache (if available and still valid)
    /// </summary>
    public async Task<T> GetAsync<T>(Func<T,DateTimeOffset> setCacheInvalidTime = null, bool invalidateCache = false) 
        where T : CachedDataBase
    {
        var endpoint = typeof(T).GetAttribute<ApiEndpointAttribute>().Endpoint;
        var cache = await _cacheService.TryGetCacheAsync<T>();
        if (cache.cache is not null && cache.isValid && !invalidateCache) return cache.cache;
        try
        {
            await using var data = await Client.GetStreamAsync(endpoint);
            var deserialized = await JsonSerializer.DeserializeAsync<T>(data, Options);
            await _cacheService.WriteCacheToDisk(deserialized, setCacheInvalidTime);
            return deserialized;
        }
        catch
        {
            // return the cache regardless as a fallback.
            return cache.cache;
        }
    }
    
    /// <summary>
    /// Get the specified type from either the API or the local cache (if available and still valid)
    /// Also gets another type that it is dependent on for setting the cache time. (usually ScheduleRoot)
    /// </summary>
    public async Task<TResult> GetAsync<TResult, TRequires>(
        Func<TRequires, TResult, DateTimeOffset> setCacheInvalidTime, 
        bool invalidateCache = false) 
        where TRequires : CachedDataBase where TResult : CachedDataBase
    {
        var requires = await GetAsync<TRequires>();
        return await GetAsync<TResult>(res => setCacheInvalidTime(requires, res), invalidateCache);
    }
}