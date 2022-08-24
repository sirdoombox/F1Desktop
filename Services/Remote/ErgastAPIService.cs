using System.Net.Http;
using System.Text.Json;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using F1Desktop.Attributes;
using F1Desktop.Enums;
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
    
    private readonly RateLimiter _hourlyRateLimiter;
    private readonly RateLimiter _secondRateLimiter;

    static ErgastAPIService()
    {
        Client = new HttpClient
        {
            BaseAddress = new Uri("https://ergast.com/api/f1/")
        };
        Options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public ErgastAPIService()
    {
        _hourlyRateLimiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions(
            200, QueueProcessingOrder.OldestFirst, 200, TimeSpan.FromHours(1)));
        _secondRateLimiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions(
            4, QueueProcessingOrder.OldestFirst, 10, TimeSpan.FromSeconds(1)));
    }
    
    /// <summary>
    /// Get the specified type from either the API or the local cache (if available and still valid)
    /// </summary>
    public async Task<(T result, ApiRequestStatus status)> GetAsync<T>() where T : ErgastApiBase
    {
        var secondLease = await _secondRateLimiter.WaitAsync();
        var hourLease = _hourlyRateLimiter.Acquire();
        if (!secondLease.IsAcquired) return (null,ApiRequestStatus.RateLimitFailureSecond);
        if (!hourLease.IsAcquired) return (null,ApiRequestStatus.RateLimitFailureHour);
        var endpoint = typeof(T).GetAttribute<ApiEndpointAttribute>().Endpoint;
        try
        {
            await using var data = await Client.GetStreamAsync(endpoint);
            var deserialized = await JsonSerializer.DeserializeAsync<T>(data, Options);
            return (deserialized,ApiRequestStatus.Success);
        }
        catch
        {
            return (null, ApiRequestStatus.Failure);
        }
    }
}