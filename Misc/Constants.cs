using System.IO;
using F1Desktop.Enums;

namespace F1Desktop.Misc;

public static class Constants
{
    public const string AppName = "F1Desktop";
    public const string AppExe = $"{AppName}.exe";
    
    public static string AppDataPath { get; }
    public static string AppCachePath { get; }
    public static string AppConfigPath { get; }
    public static string AppLogsPath { get; }

    public const string RegistryStartupSubKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

    public const string GitHubRepoUrl = "https://github.com/sirdoombox/F1Desktop";
    
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

    static Constants()
    {
        AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
        AppCachePath = Path.Combine(AppDataPath, "Cache");
        AppConfigPath = Path.Combine(AppDataPath, "Data");
        AppLogsPath = Path.Combine(AppDataPath, "Logs");
    }
}