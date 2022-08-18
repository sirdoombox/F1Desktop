using System.Text.Json.Serialization;
using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.ErgastAPI.ConstructorStandings;

[Filename("Constructor.dat")]
[CacheDuration(days: 1)]
public class ConstructorStandingsRoot : CachedDataBase
{
    [JsonPropertyName("MRData")]
    public ConstructorStandingsData Data { get; set; }
}