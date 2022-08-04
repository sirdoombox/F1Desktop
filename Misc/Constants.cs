using F1Desktop.Enums;

namespace F1Desktop.Misc;

public static class Constants
{
    public const string AppName = "F1Desktop";

    public static readonly IReadOnlyDictionary<string, string> BaseNewsFeeds = new Dictionary<string, string>
    {
        // { "Formula 1", "https://www.formula1.com/content/fom-website/en/latest/all.xml" }, // - Leave this one out for now (no publish time)
        { "WTF1", "https://wtf1.com/feed/" },
        { "Racer", "https://racer.com/f1/feed/" }
    };

    public static readonly IReadOnlyList<SessionType> SprintWeekendOrder = new[]
    {
        SessionType.FirstPractice,
        SessionType.Qualifying,
        SessionType.SecondPractice,
        SessionType.Sprint,
        SessionType.Race
    };

    public static readonly IReadOnlyList<SessionType> NormalWeekendOrder = new[]
    {
        SessionType.FirstPractice,
        SessionType.SecondPractice,
        SessionType.ThirdPractice,
        SessionType.Qualifying,
        SessionType.Race
    };
}