namespace F1Desktop.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CacheDurationAttribute : Attribute
{
    public TimeSpan CacheValidFor { get; }

    public CacheDurationAttribute(int days = 0, int hours = 0, int minutes = 0)
    {
        CacheValidFor = new TimeSpan(days, hours, minutes, 0);
    }
}