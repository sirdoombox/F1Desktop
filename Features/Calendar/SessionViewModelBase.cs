using System.Windows;
using F1Desktop.Services.Local;
using Stylet;

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
    
    public bool IsUpcoming { get; }
    
    private bool _use24HourClock;
    public bool Use24HourClock
    {
        get => _use24HourClock;
        set => SetAndNotify(ref _use24HourClock, value);
    }

    public SessionViewModelBase(DateTimeOffset sessionTime, GlobalConfigService cfg)
    {
        SessionTime = sessionTime;
        IsUpcoming = sessionTime > DateTimeOffset.Now;
        Use24HourClock = cfg.Use24HourClock;
        cfg.OnPropertyChanged += _ => Use24HourClock = cfg.Use24HourClock;
    }

}