using F1Desktop.Misc.Extensions;
using F1Desktop.Models.ErgastAPI.DriverStandings;
using F1Desktop.Models.Resources;
using Stylet;

namespace F1Desktop.Features.Standings;

public class DriverStandingsViewModel : PropertyChangedBase
{
    public BindableCollection<DriverViewModel> Drivers { get; } = new();

    public void PassDriverStandings(List<DriverStanding> standings, IEnumerable<CountryData> countryData)
    {
        foreach (var standing in standings)
            Drivers.Add(new DriverViewModel(standing,
                countryData.GetCountryCodeForNationality(standing.Driver.Nationality)));
    }
}