namespace F1Desktop.Models.ErgastAPI.Schedule;

public class Race : Session
{
    public string season { get; set; }
    public string round { get; set; }
    public string url { get; set; }
    public string raceName { get; set; }
    public Circuit Circuit { get; set; }
    public Session FirstPractice { get; set; }
    public Session SecondPractice { get; set; }
    public Session ThirdPractice { get; set; }
    public Session Qualifying { get; set; }
    public Session Sprint { get; set; }
}