using F1Desktop.Enums;

namespace F1Desktop.Misc;

public static class Constants
{
    public const string AppName = "F1Desktop";

    public const string RegistryStartupSubKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
    
    public const string ShortTimeFormat12Hour = "hh\\:mmtt";
    public const string ShortTimeFormat24Hour = "HH\\:mm";
    public const string DateFormat = "ddd\\, d MMMM";
    public const string LongTimeFormat12Hour = $"{DateFormat} \\- {ShortTimeFormat12Hour}";
    public const string LongTimeFormat24Hour = $"{DateFormat} \\- {ShortTimeFormat24Hour}";

    public const int GlobalToolTipDelay = 100;
        
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