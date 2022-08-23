using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using F1Desktop.Attributes;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.Base;
using F1Desktop.Models.ErgastAPI.ConstructorStandings;
using F1Desktop.Models.ErgastAPI.DriverStandings;
using F1Desktop.Models.ErgastAPI.Schedule;
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
    
    public async Task<T> GetAsync<T>(bool invalidateCache = false) where T : CachedDataBase
    {
        var endpoint = typeof(T).GetAttribute<ApiEndpointAttribute>().Endpoint;
        var cache = await _cacheService.TryGetCacheAsync<T>();
        if (cache.cache is not null && cache.isValid && !invalidateCache) return cache.cache;
        try
        {
            await using var data = await Client.GetStreamAsync(endpoint);
            var deserialized = await JsonSerializer.DeserializeAsync<T>(data, Options);
            await _cacheService.WriteCacheToDisk(deserialized);
            return deserialized;
        }
        catch
        {
            // return the cache regardless as a fallback.
            return cache.cache;
        }
    }
}