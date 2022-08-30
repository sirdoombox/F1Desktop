using F1Desktop.Enums;

namespace F1Desktop.Services.Interfaces;

public interface ITimeService
{
    public DateTimeOffset OffsetNow { get; }
    
    public void RegisterTickCallback(Every every, Action<DateTimeOffset> callback);

    public DateTimeOffset DaysBeforeNow(int days);
    public DateTimeOffset StartOfDay(DateTimeOffset time);
    public DateTimeOffset StartOfWeek(DateTimeOffset time);
    public DateTimeOffset WeeksBeforeNow(int weeks);
    public DateTimeOffset GetWeekStart(DateTimeOffset time);
    
    public bool IsUpcoming(DateTimeOffset time);
    public bool HasPassed(DateTimeOffset time);
    public bool IsToday(DateTimeOffset time);
    public bool IsWithinMinutesBefore(DateTimeOffset isThisTimeWithin, DateTimeOffset ofTime, int minutes);
    public bool IsSameDay(DateTimeOffset timeOne, DateTimeOffset timeTwo);
    public bool IsSameWeek(DateTimeOffset timeOne, DateTimeOffset timeTwo);
}