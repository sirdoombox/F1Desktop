using F1Desktop.Enums;
using F1Desktop.Models.Misc;
using F1Desktop.Services.Base;
using F1Desktop.Services.Interfaces;
using FluentScheduler;

namespace F1Desktop.Services.Local;

public class TimeService : ServiceBase, ITimeDebug
{
    public DateTimeOffset DebugTime { get; set; }
    
    public DateTimeOffset OffsetNow => _isDebug ? DebugTime : DateTimeOffset.Now;

    public Action<DateTimeOffset> OneSecondTick { get; private set; }
    public Action<DateTimeOffset> TenSecondTick { get; private set; }

    private readonly bool _isDebug;
    
    public TimeService(StartupState startupState)
    {
        DebugTime = DateTimeOffset.Now;
        _isDebug = startupState.Debug;
        if (_isDebug) return; // prevent registering jobs when debugging.
        JobManager.Initialize();
        JobManager.Start();
        JobManager.AddJob(() => OneSecondTick?.Invoke(DateTimeOffset.Now), s => s.ToRunEvery(1).Seconds());
        JobManager.AddJob(() => TenSecondTick?.Invoke(DateTimeOffset.Now), s => s.ToRunEvery(10).Seconds());
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

    public DateTimeOffset GetWeekStart(DateTimeOffset sessionStart)
    {
        var diff = (7 + (sessionStart.DayOfWeek - DayOfWeek.Monday)) % 7;
        return sessionStart - new TimeSpan(diff, sessionStart.Hour, sessionStart.Minute, sessionStart.Second);
    }
}