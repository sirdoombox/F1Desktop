using F1Desktop.Misc;

namespace F1Desktop.Features.Standings;

public class StandingViewModel : PropertyChangedBase
{
    public string GivenName { get; init; }
    public string FamilyName { get; init; }
    public ushort Position { get; init; }
    public string ConstructorName { get; init; }
    public ushort Points { get; init; }
    public int LeaderPointsDiff { get; init; }
    public int PointsDiff { get; init; }
    public string CountryCode { get; init; }
    public string Nationality { get; init; }
    public string WikiUrl { get; init; }

    public void OpenWiki()
    {
        UrlHelper.Open(WikiUrl);
    }
}