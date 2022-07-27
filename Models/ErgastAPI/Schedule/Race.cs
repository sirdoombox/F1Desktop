namespace F1Desktop.Models.ErgastAPI;

public class Race
{
    public string season { get; set; }
    public string round { get; set; }
    public string url { get; set; }
    public string raceName { get; set; }
    public Circuit Circuit { get; set; }
    public string date { get; set; }
    public string time { get; set; }
    public FirstPractice FirstPractice { get; set; }
    public SecondPractice SecondPractice { get; set; }
    public ThirdPractice ThirdPractice { get; set; }
    public Qualifying Qualifying { get; set; }
    public Sprint Sprint { get; set; }
}