using System.Text.Json.Serialization;
using F1Desktop.Enums;

namespace F1Desktop.Models.ErgastAPI.Schedule;

public class Race : Session
{
    public ushort Season { get; set; }
    public ushort Round { get; set; }
    public string Url { get; set; }
    public string RaceName { get; set; }
    public Circuit Circuit { get; set; }
    public Session FirstPractice { get; set; }
    public Session SecondPractice { get; set; }
    public Session ThirdPractice { get; set; }
    public Session Qualifying { get; set; }
    public Session Sprint { get; set; }

    [JsonIgnore]
    public bool IsSprintWeekend => Sprint is not null;

    [JsonIgnore]
    public IReadOnlyDictionary<SessionType, Session> Sessions => new Dictionary<SessionType, Session>
    {
        { SessionType.FirstPractice, FirstPractice },
        { SessionType.SecondPractice, SecondPractice },
        { SessionType.ThirdPractice, ThirdPractice },
        { SessionType.Qualifying, Qualifying },
        { SessionType.Sprint, Sprint },
        { SessionType.Race, this }
    };
}