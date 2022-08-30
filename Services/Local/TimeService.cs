using F1Desktop.Enums;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.Misc;
using F1Desktop.Services.Base;
using F1Desktop.Services.Interfaces;
using FluentScheduler;

namespace F1Desktop.Services.Local;

public class TimeService : ServiceBase, ITimeServiceDebug
{
    public DateTimeOffset DebugTime { get; set; }
    
    public DateTimeOffset OffsetNow => _isDebug ? DebugTime : DateTimeOffset.Now.RoundDown(TimeSpan.FromMinutes(1));

    public Action<DateTimeOffset> OneSecondTick { get; private set; }
    public Action<DateTimeOffset> TenSecondTick { get; private set; }

    private readonly bool _isDebug;
    
    public TimeService(StartupState startupState)
    {
        DebugTime = OffsetNow; // Set debug time to now before we actually set debug state.
        _isDebug = startupState.Debug;
        if (_isDebug) return; // prevent registering jobs when debugging.
        JobManager.Initialize();
        JobManager.Start();
        JobManager.AddJob(() => OneSecondTick?.Invoke(OffsetNow), s => s.ToRunEvery(1).Seconds());
        JobManager.AddJob(() => TenSecondTick?.Invoke(OffsetNow), s => s.ToRunEvery(10).Seconds());
    }

    public void RegisterTickCallback(Every every, Action<DateTimeOffset> callback)
    {
        switch (every)
        {
            case Every.OneSecond:
                OneSecondTick += callback;
                break;
            case Every.TenSeconds:
                TenSecondTick += callback;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(every), every, null);
        }
    }

    public bool IsUpcoming(DateTimeOffset time) => 
        time > OffsetNow;

    public bool HasPassed(DateTimeOffset time) =>
        time <= OffsetNow;

    public bool IsToday(DateTimeOffset time) =>
        IsSameDay(time, OffsetNow);

    public bool IsWithinMinutesBefore(DateTimeOffset isThisTimeWithin, DateTimeOffset ofTime, int minutes) =>
        isThisTimeWithin <= ofTime && isThisTimeWithin >= ofTime.AddMinutes(minutes);
    
    public bool IsSameDay(DateTimeOffset timeOne, DateTimeOffset timeTwo) =>
        timeOne.Date == timeTwo.Date;

    public bool IsSameWeek(DateTimeOffset timeOne, DateTimeOffset timeTwo) =>
        GetWeekStart(timeOne).Date == GetWeekStart(timeTwo).Date;

    public DateTimeOffset DaysBeforeNow(int days) => 
        (OffsetNow - TimeSpan.FromDays(days)).RoundDown(TimeSpan.FromDays(1));

    public DateTimeOffset StartOfDay(DateTimeOffset time) =>
        time.RoundDown(TimeSpan.FromDays(1));

    public DateTimeOffset StartOfWeek(DateTimeOffset time)
    {
        var diff = (7 + (time.DayOfWeek - DayOfWeek.Monday)) % 7;
        return time - new TimeSpan(diff, time.Hour, time.Minute, time.Second);
    }

    public DateTimeOffset WeeksBeforeNow(int weeks) => 
        (OffsetNow - TimeSpan.FromDays(weeks * 7)).RoundDown(TimeSpan.FromDays(1));

    public DateTimeOffset GetWeekStart(DateTimeOffset time)
    {
        var diff = (7 + (time.DayOfWeek - DayOfWeek.Monday)) % 7;
        return time - new TimeSpan(diff, time.Hour, time.Minute, time.Second);
    }
}