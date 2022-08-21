using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.Config;

[Filename("Calendar.cfg")]
public sealed class CalendarConfig : ConfigBase
{
    public bool ShowPreviousRaces { get; set; }
    public bool EnableNotifications { get; set; }

    public CalendarConfig()
    {
        Default();
    }

    public override void Default()
    {
        ShowPreviousRaces = false;
        EnableNotifications = true;
    }
}