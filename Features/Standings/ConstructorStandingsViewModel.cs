using F1Desktop.Misc.Extensions;
using F1Desktop.Models.ErgastAPI.ConstructorStandings;
using F1Desktop.Models.Resources;
using Stylet;

namespace F1Desktop.Features.Standings;

public class ConstructorStandingsViewModel : PropertyChangedBase
{
    public BindableCollection<ConstructorViewModel> Constructors { get; } = new();

    public void PassConstructorStandings(List<ConstructorStanding> standings, IEnumerable<CountryData> data)
    {
        foreach (var standing in standings)
            Constructors.Add(new ConstructorViewModel(standing, data.GetCountryCodeForNationality(standing.Constructor.Nationality)));
    }
}