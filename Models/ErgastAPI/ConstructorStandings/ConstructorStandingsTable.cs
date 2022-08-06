using F1Desktop.Models.ErgastAPI.Shared;

namespace F1Desktop.Models.ErgastAPI.ConstructorStandings;

public class ConstructorStandingsTable : StandingsTableBase
{
    public List<StandingsList> StandingsLists { get; set; }
}