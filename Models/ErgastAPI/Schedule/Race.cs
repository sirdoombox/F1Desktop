using System.Text.Json.Serialization;

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

    public bool IsSprintWeekend => Sprint is not null;
    
    [JsonIgnore]
    public IReadOnlyDictionary<string, Session> SprintWeekend => new Dictionary<string, Session>()
    {
        { "First Practice", FirstPractice },
        { "Qualifying", Qualifying },
        { "Second Practice", SecondPractice },
        { "Sprint", Sprint },
        { "Race", this }
    };
    
    [JsonIgnore]
    public IReadOnlyDictionary<string, Session> NormalWeekend => new Dictionary<string, Session>()
    {
        { "First Practice", FirstPractice },
        { "Second Practice", SecondPractice },
        { "Third Practice", ThirdPractice },
        { "Qualifying", Qualifying },
        { "Race", this }
    };
}