using F1Desktop.Models.ErgastAPI.Shared;

namespace F1Desktop.Models.ErgastAPI.ConstructorStandings;

public class StandingsList : StandingsListBase
{
    public List<ConstructorStanding> ConstructorStandings { get; set; }
}