namespace F1Desktop.Misc.Extensions;

public static class DateTimeOffsetExtensions
{
    public static DateTimeOffset RoundUp(this DateTimeOffset dt, TimeSpan d)
    {
        var modTicks = dt.Ticks % d.Ticks;
        var delta = modTicks != 0 ? d.Ticks - modTicks : 0;
        return new DateTimeOffset(dt.Ticks + delta, dt.Offset);
    }

    public static DateTimeOffset RoundDown(this DateTimeOffset dt, TimeSpan d)
    {
        var delta = dt.Ticks % d.Ticks;
        return new DateTimeOffset(dt.Ticks - delta, dt.Offset);
    }

    public static string ToShortString(this DateTimeOffset dt, bool is24Hour) =>
        dt.ToString(is24Hour ? Constants.UI.ShortTimeFormat24Hour : Constants.UI.ShortTimeFormat12Hour);
    
    public static string ToLongString(this DateTimeOffset dt, bool is24Hour) =>
        dt.ToString(is24Hour ? Constants.UI.LongTimeFormat24Hour : Constants.UI.LongTimeFormat12Hour);

    public static string ToShortString(this DateTime dt, bool is24Hour) => 
        dt.ToString(is24Hour ? Constants.UI.ShortTimeFormat24Hour : Constants.UI.ShortTimeFormat12Hour);
    
    public static string ToLongString(this DateTime dt, bool is24Hour) =>
        dt.ToString(is24Hour ? Constants.UI.LongTimeFormat24Hour : Constants.UI.LongTimeFormat12Hour);
}