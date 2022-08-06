using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using F1Desktop.Models.Base;
using F1Desktop.Models.ErgastAPI.ConstructorStandings;
using F1Desktop.Models.ErgastAPI.DriverStandings;
using F1Desktop.Models.ErgastAPI.Schedule;
using F1Desktop.Services.Interfaces;
using JetBrains.Annotations;

namespace F1Desktop.Services;

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

    public Task<ScheduleRoot> GetScheduleAsync(bool invalidateCache = false) =>
        GetAsync<ScheduleRoot>("current.json", invalidateCache);

    public Task<DriverStandingsRoot> GetDriverStandingsAsync(bool invalidateCache = false) =>
        GetAsync<DriverStandingsRoot>("current/driverStandings.json", invalidateCache);

    public Task<ConstructorStandingsRoot> GetConstructorStandingsAsync(bool invalidateCache = false) =>
        GetAsync<ConstructorStandingsRoot>("current/constructorStandings.json", invalidateCache);

    private async Task<T> GetAsync<T>(string endpoint, bool invalidateCache) where T : CachedDataBase
    {
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