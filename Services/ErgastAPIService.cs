using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using F1Desktop.Models.ErgastAPI;
using F1Desktop.Models.ErgastAPI.Schedule;
using F1Desktop.Services.Interfaces;

namespace F1Desktop.Services;

public class ErgastAPIService
{
    private static readonly HttpClient _client;

    private readonly IDataCacheService _cacheService;

    static ErgastAPIService()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://ergast.com/api/f1/")
        };
    }

    public ErgastAPIService(IDataCacheService cacheService)
    {
        _cacheService = cacheService;
    }
    
    public async Task<ScheduleRoot?> GetScheduleAsync(bool invalidateCache = false)
    {
        var cache = await _cacheService.TryGetCacheAsync<ScheduleRoot>();
        if (cache.cache is not null && cache.isValid && !invalidateCache) return cache.cache;
        await using var data = await _client.GetStreamAsync("current.json");
        var deserialized = await JsonSerializer.DeserializeAsync<ScheduleRoot>(data);
        await _cacheService.WriteCacheToDisk(deserialized);
        return deserialized;
    }
}