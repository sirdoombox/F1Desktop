using System.IO;
using F1Desktop.Enums;

namespace F1Desktop.Misc;

public static class Constants
{
    public static class App
    {
        public const string Name = "F1Desktop";
        public const string Exe = $"{Name}.exe";

        public static string DataPath { get; }
        public static string ConfigPath { get; }
        public static string LogsPath { get; }
        
        static App()
        {
            DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Name);
            ConfigPath = Path.Combine(DataPath, "Data");
            LogsPath = Path.Combine(DataPath, "Logs");
        }
    }

    public static class UI
    {
        public const string ShortTimeFormat12Hour = "hh\\:mmtt";
        public const string ShortTimeFormat24Hour = "HH\\:mm";
        public const string DateFormat = "ddd\\, d MMMM";
        public const string LongTimeFormat12Hour = $"{DateFormat} \\- {ShortTimeFormat12Hour}";
        public const string LongTimeFormat24Hour = $"{DateFormat} \\- {ShortTimeFormat24Hour}";
        public const int GlobalToolTipDelay = 100;
    }

    public static class Url
    {
        public const string GitHubRepo = "https://github.com/sirdoombox/F1Desktop";
    }

    public static class Misc
    {
        public const string RegistryStartupSubKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
    }

    public static class F1
    {
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
}