using System.ComponentModel;

namespace F1Desktop.Enums;

public enum SessionType
{
    [Description("First Practice")]
    FirstPractice,

    [Description("Second Practice")]
    SecondPractice,

    [Description("Third Practice")]
    ThirdPractice,
    Qualifying,
    Sprint,
    Race
}