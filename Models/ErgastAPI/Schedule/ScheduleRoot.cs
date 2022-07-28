using System.Text.Json.Serialization;
using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.ErgastAPI.Schedule;

[Filename("Schedule.dat")]
[CacheDuration(days:7)]
public class ScheduleRoot : CachedDataBase
{
    [JsonPropertyName("MRData")]
    public ScheduleData ScheduleData { get; set; }
}