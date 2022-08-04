using F1Desktop.Enums;

namespace F1Desktop.Misc;

public static class Constants
{
    public const string AppName = "F1Desktop";

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