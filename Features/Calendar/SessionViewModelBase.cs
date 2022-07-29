using System;
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

    public SessionViewModelBase(DateTimeOffset sessionTime)
    {
        SessionTime = sessionTime;
    }
}