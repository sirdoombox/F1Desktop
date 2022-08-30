using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.Config;

[Filename("Calendar.cfg")]
public sealed class CalendarConfig : ConfigBase
{
    public bool ShowPreviousRaces { get; set; }
    public bool EnableNotifications { get; set; }
    public bool EnableThirtyMinuteNotifications { get; set; }
    public bool EnableDayNotifications { get; set; }
    public bool EnableRaceWeekNotifications { get; set; }
    // Persist state for race week notification so it's only sent once per week/day
    public DateTimeOffset RaceWeekNotificationSentFor { get; set; }
    public DateTimeOffset DayNotificationSentFor { get; set; }

    public CalendarConfig()
    {
        Default();
    }

    public override void Default()
    {
        ShowPreviousRaces = false;
        EnableNotifications = true;
        EnableThirtyMinuteNotifications = true;
        EnableDayNotifications = true;
        EnableRaceWeekNotifications = true;
        DayNotificationSentFor = DateTimeOffset.MinValue;
        RaceWeekNotificationSentFor = DateTimeOffset.MinValue;
    }
}