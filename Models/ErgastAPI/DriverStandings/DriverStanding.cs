using F1Desktop.Models.ErgastAPI.Shared;

namespace F1Desktop.Models.ErgastAPI.DriverStandings;

public class DriverStanding : StandingBase
{
    public Driver Driver { get; set; }
    public List<Constructor> Constructors { get; set; }
}