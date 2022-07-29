using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.Config;

[Filename("Calendar.cfg")]
public class CalendarConfig : ConfigBase
{
    public bool ShowPreviousRaces { get; set; }

    public CalendarConfig()
    {
        ShowPreviousRaces = false;
    }
}