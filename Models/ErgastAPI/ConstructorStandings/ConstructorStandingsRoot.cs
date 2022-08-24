using System.Text.Json.Serialization;
using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.ErgastAPI.ConstructorStandings;

[ApiEndpoint("current/constructorStandings.json")]
[Filename("Constructor.dat")]
public class ConstructorStandingsRoot : ErgastApiBase
{
    [JsonPropertyName("MRData")]
    public ConstructorStandingsData Data { get; set; }
}