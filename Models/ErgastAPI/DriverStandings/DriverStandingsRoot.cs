using System.Text.Json.Serialization;
using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.ErgastAPI.DriverStandings;

[Filename("Drivers.dat")]
[CacheDuration(days: 1)]
public class DriverStandingsRoot : CachedDataBase
{
    [JsonPropertyName("MRData")]
    public DriverStandingsData Data { get; set; }
}