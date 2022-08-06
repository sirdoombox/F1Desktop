using F1Desktop.Misc;
using F1Desktop.Models.ErgastAPI.ConstructorStandings;
using Stylet;

namespace F1Desktop.Features.Standings;

public class ConstructorViewModel : PropertyChangedBase
{
    public string Name { get; }
    public ushort Position { get; }
    public ushort Points { get; }
    public string CountryCode { get; }
    public string Nationality { get; }
    private readonly string _wikiUrl;
    
    public ConstructorViewModel(ConstructorStanding standing, string countryCode)
    {
        Name = standing.Constructor.Name;
        Position = standing.Position;
        Points = standing.Points;
        CountryCode = countryCode;
        Nationality = standing.Constructor.Nationality;
        _wikiUrl = standing.Constructor.Url;
    }

    public void ConstructorClicked()
    {
        UrlHelper.Open(_wikiUrl);
    }
}