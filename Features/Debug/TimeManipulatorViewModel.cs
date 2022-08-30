using F1Desktop.Features.Debug.Base;
using F1Desktop.Misc.Extensions;
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
    
    private int _dayIncrement;
    public int DayIncrement
    {
        get => _dayIncrement;
        set => SetAndNotify(ref _dayIncrement, value);
    }
    
    private int _hourIncrement;
    public int HourIncrement
    {
        get => _hourIncrement;
        set => SetAndNotify(ref _hourIncrement, value);
    }
    
    private int _minuteIncrement;
    public int MinuteIncrement
    {
        get => _minuteIncrement;
        set => SetAndNotify(ref _minuteIncrement, value);
    }

    private DateTimeOffset OffsetTime => new(Year, Month, Day, Hour, Minute, 0, DateTimeOffset.Now.Offset);
    
    private readonly ITimeServiceDebug _time;

    public TimeManipulatorViewModel(ITimeServiceDebug time) : base("Time", PackIconMaterialKind.Clock)
    {
        _time = time;
        SetTime(DateTimeOffset.Now.RoundDown(TimeSpan.FromMinutes(1)));
    }

    private void SetTime(DateTimeOffset time)
    {
        Year = time.Year;
        Day = time.Day;
        Month = time.Month;
        Hour = time.Hour;
        Minute = time.Minute;
        DayIncrement = HourIncrement = MinuteIncrement = 1;
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

    public void MoveDay(bool isPositive) => 
        SetTime(OffsetTime.AddDays(isPositive ? DayIncrement : -DayIncrement));
    
    public void MoveHour(bool isPositive) => 
        SetTime(OffsetTime.AddHours(isPositive ? HourIncrement : -HourIncrement));
    
    public void MoveMinute(bool isPositive) => 
        SetTime(OffsetTime.AddMinutes(isPositive ? MinuteIncrement : -MinuteIncrement));
}