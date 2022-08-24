using System.Text.Json.Serialization;
using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.ErgastAPI.DriverStandings;

[ApiEndpoint("current/driverStandings.json")]
[Filename("Drivers.dat")]
public class DriverStandingsRoot : ErgastApiBase
{
    [JsonPropertyName("MRData")]
    public DriverStandingsData Data { get; set; }
}