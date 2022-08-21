namespace F1Desktop.Misc.Extensions;

public static class TimespanExtensions
{
    /// <summary>
    /// Returns the number of weeks represented by this timespan.
    /// </summary>
    public static int Weeks(this TimeSpan ts)
    {
        var res = ts.Days / 7;
        return res;
    }

    /// <summary>
    /// Returns the number of days (remaining after weeks) represented by this timespan.
    /// </summary>
    public static int Days(this TimeSpan ts) => ts.Days % 7;
}