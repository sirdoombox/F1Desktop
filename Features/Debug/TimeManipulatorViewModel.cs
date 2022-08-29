using F1Desktop.Features.Debug.Base;
using F1Desktop.Services.Interfaces;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Debug;

public class TimeManipulatorViewModel : DebugFeatureBase
{
    private int _year;
    public int Year
    {
        get => _year;
        set => SetAndNotify(ref _year, value);
    }
    
    private int _month;
    public int Month
    {
        get => _month;
        set => SetAndNotify(ref _month, value);
    }
    
    private int _day;
    public int Day
    {
        get => _day;
        set => SetAndNotify(ref _day, value);
    }
    
    private int _hour;
    public int Hour
    {
        get => _hour;
        set => SetAndNotify(ref _hour, value);
    }
    
    private int _minute;
    public int Minute
    {
        get => _minute;
        set => SetAndNotify(ref _minute, value);
    }

    private DateTimeOffset OffsetTime => new(Year, Month, Day, Hour, Minute, 0, TimeSpan.Zero);
    
    private readonly ITimeDebug _time;

    public TimeManipulatorViewModel(ITimeDebug time) : base("Time", PackIconMaterialKind.Clock)
    {
        _time = time;
        SetTime(DateTimeOffset.Now);
    }

    private void SetTime(DateTimeOffset time)
    {
        Year = time.Year;
        Day = time.Day;
        Month = time.Month;
        Hour = time.Hour;
        Minute = time.Minute;
        _time.DebugTime = OffsetTime;
        _time.OneSecondTick?.Invoke(_time.OffsetNow);
        _time.TenSecondTick?.Invoke(_time.OffsetNow);
    }

    public void SetTime()
    {
        _time.DebugTime = OffsetTime;
        _time.OneSecondTick?.Invoke(_time.OffsetNow);
        _time.TenSecondTick?.Invoke(_time.OffsetNow);
    }

    public void MoveHour(bool isForward) => SetTime(OffsetTime.AddHours(isForward ? 1 : -1));

    public void MoveDay(bool isForward) => SetTime(OffsetTime.AddDays(isForward ? 1 : -1));
}