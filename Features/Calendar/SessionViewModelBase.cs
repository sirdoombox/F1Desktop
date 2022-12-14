using F1Desktop.Services.Interfaces;
using F1Desktop.Services.Local;

namespace F1Desktop.Features.Calendar;

public abstract class SessionViewModelBase : PropertyChangedBase
{
    private bool _isNext;
    public bool IsNext
    {
        get => _isNext;
        set => SetAndNotify(ref _isNext, value);
    }

    public DateTimeOffset SessionTime { get; }

    private bool _isUpcoming;
    public bool IsUpcoming
    {
        get => _isUpcoming;
        set => SetAndNotify(ref _isUpcoming, value);
    }

    private bool _use24HourClock;
    public bool Use24HourClock
    {
        get => _use24HourClock;
        set => SetAndNotify(ref _use24HourClock, value);
    }

    public SessionViewModelBase(DateTimeOffset sessionTime, GlobalConfigService cfg, ITimeService time)
    {
        SessionTime = sessionTime;
        IsUpcoming = time.IsUpcoming(sessionTime);
        Use24HourClock = cfg.Use24HourClock;
        cfg.OnPropertyChanged += _ => Use24HourClock = cfg.Use24HourClock;
    }
}