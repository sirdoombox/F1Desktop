using AdonisUI.ViewModels;
using F1Desktop.Misc;
using F1Desktop.Models.ErgastAPI.DriverStandings;

namespace F1Desktop.Features.Standings;

public class DriverViewModel : PropertyChangedBase
{
    public string GivenName { get; }
    public string FamilyName { get; }
    public ushort Position { get; }
    public string ConstructorName { get; }
    public ushort Points { get; }
    public string CountryCode { get; }
    public string Nationality { get; }
    
    private readonly string _wikiUrl;
    
    public DriverViewModel(DriverStanding standing, string countryCode)
    {
        GivenName = standing.Driver.GivenName;
        FamilyName = standing.Driver.FamilyName;
        Position = standing.Position;
        ConstructorName = standing.Constructors[0].Name;
        Points = standing.Points;
        CountryCode = countryCode;
        Nationality = standing.Driver.Nationality;
        _wikiUrl = standing.Driver.Url;
    }

    public void DriverClicked()
    {
        UrlHelper.Open(_wikiUrl);
    }
}