using System.Windows;
using Stylet;

namespace F1Desktop.Features.Calendar;

public abstract class SessionViewModelBase : PropertyChangedBase, IViewAware
{
    private bool _isNext;
    public bool IsNext
    {
        get => _isNext;
        set => SetAndNotify(ref _isNext, value);
    }

    public DateTimeOffset SessionTime { get; }
    
    public bool IsUpcoming { get; }

    public SessionViewModelBase(DateTimeOffset sessionTime)
    {
        SessionTime = sessionTime;
        IsUpcoming = sessionTime > DateTimeOffset.Now;
    }

    public void AttachView(UIElement view) => View = view;
    public UIElement View { get; private set; }
}