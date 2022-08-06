using F1Desktop.Models.ErgastAPI.Shared;

namespace F1Desktop.Models.ErgastAPI.DriverStandings;

public class StandingsList : StandingsListBase
{
    public List<DriverStanding> DriverStandings { get; set; }
}