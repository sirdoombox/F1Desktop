using F1Desktop.Misc.Extensions;
using F1Desktop.Models.ErgastAPI.ConstructorStandings;
using F1Desktop.Models.ErgastAPI.DriverStandings;
using F1Desktop.Models.ErgastAPI.Shared;
using F1Desktop.Models.Resources;

namespace F1Desktop.Features.Standings;

public class StandingsTableViewModel : PropertyChangedBase
{
    public BindableCollection<StandingViewModel> Standings { get; } = new();
    
    private bool _pointsDiffFromLeader;
    public bool PointsDiffFromLeader
    {
        get => _pointsDiffFromLeader;
        set => SetAndNotify(ref _pointsDiffFromLeader, value);
    }

    public void InitStandings<T>(IEnumerable<T> standings, IEnumerable<CountryData> countryData) where T : StandingBase
    {
        Standings.Clear();
        var leader = standings.First();
        var prev = leader;
        foreach (var standing in standings)
        {
            var givenName = string.Empty;
            var familyName = string.Empty;
            var constructorName = string.Empty;
            var nationality = string.Empty;
            var wikiUrl = string.Empty;
            var countryCode = string.Empty;
            switch (standing)
            {
                case DriverStanding ds:
                    givenName = ds.Driver.GivenName;
                    familyName = ds.Driver.FamilyName;
                    constructorName = ds.Constructors[0].Name;
                    nationality = ds.Driver.Nationality;
                    wikiUrl = ds.Driver.Url;
                    countryCode = countryData.GetCountryCodeForNationality(ds.Driver.Nationality);
                    break;
                case ConstructorStanding cs:
                    givenName = cs.Constructor.Name;
                    nationality = cs.Constructor.Nationality;
                    wikiUrl = cs.Constructor.Url;
                    countryCode = countryData.GetCountryCodeForNationality(cs.Constructor.Nationality);
                    break;
            }
            Standings.Add(new StandingViewModel
            {
                GivenName = givenName,
                FamilyName = familyName,
                Position = standing.Position,
                ConstructorName = constructorName,
                Points = standing.Points,
                CountryCode = countryCode,
                Nationality = nationality,
                LeaderPointsDiff = leader.Points - standing.Points,
                PointsDiff = prev.Points - standing.Points,
                WikiUrl = wikiUrl
            });
            prev = standing;
        }
    }
}