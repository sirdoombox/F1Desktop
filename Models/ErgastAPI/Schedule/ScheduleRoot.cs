using System.Text.Json.Serialization;
using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.ErgastAPI.Schedule;

[ApiEndpoint("current.json")]
[Filename("Schedule.dat")]
public class ScheduleRoot : ErgastApiBase
{
    [JsonPropertyName("MRData")]
    public ScheduleData ScheduleData { get; set; }
}