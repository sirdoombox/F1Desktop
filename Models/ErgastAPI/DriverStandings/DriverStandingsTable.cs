using F1Desktop.Models.ErgastAPI.Shared;

namespace F1Desktop.Models.ErgastAPI.DriverStandings;

public class DriverStandingsTable : StandingsTableBase
{
    public List<StandingsList> StandingsLists { get; set; }
}