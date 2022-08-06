using F1Desktop.Misc.Extensions;
using F1Desktop.Models.ErgastAPI.ConstructorStandings;
using F1Desktop.Models.ErgastAPI.DriverStandings;
using F1Desktop.Models.Resources;
using Stylet;
using PropertyChangedBase = AdonisUI.ViewModels.PropertyChangedBase;

namespace F1Desktop.Features.Standings;

public class StandingsTableViewModel : PropertyChangedBase
{
    public BindableCollection<StandingViewModel> Standings { get; } = new();

    public void PassDriverStandings(IEnumerable<DriverStanding> standings, IEnumerable<CountryData> countryData)
    {
        foreach (var standing in standings)
            Standings.Add(new StandingViewModel(standing,
                countryData.GetCountryCodeForNationality(standing.Driver.Nationality)));
    }

    public void PassConstructorStandings(IEnumerable<ConstructorStanding> standings, IEnumerable<CountryData> countryData)
    {
        foreach (var standing in standings)
            Standings.Add(new StandingViewModel(standing, 
                countryData.GetCountryCodeForNationality(standing.Constructor.Nationality)));
    }
}