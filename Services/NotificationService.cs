using FluentScheduler;
using H.NotifyIcon;

namespace F1Desktop.Services;

public class NotificationService
{
    private TaskbarIcon _icon;
    private readonly Func<TaskbarIcon> _tryGetIcon;
    private readonly SortedSet<Notification> _scheduled = new();

    public NotificationService(Func<TaskbarIcon> tryGetIcon)
    {
        JobManager.AddJob(Tick, s => s.ToRunEvery(10).Seconds());
        _tryGetIcon = tryGetIcon;
    }

    public void ShowNotification(string title, string message)
    {
        _icon ??= _tryGetIcon();
        _icon?.ShowNotification(title,message);
    }

    public Notification ScheduleNotification(DateTimeOffset time, string title, string message)
    {
        var notification = new Notification(time, title, message);
        _scheduled.Add(notification);
        return notification;
    }
    
    public void CancelNotification(Notification notification) => _scheduled.Remove(notification);
    
    // call this once every 10 seconds (or whatever precision is needed)
    public void Tick()
    {
        var first = _scheduled.Min;
        if (first is null) return;
        if (first.Time > DateTimeOffset.Now) return;
        ShowNotification(first.Title, first.Message);
        _scheduled.Remove(first);
    }

    public class Notification : IComparable
    {
        public DateTimeOffset Time { get; }
        public string Title { get; }
        public string Message { get; }

        public Notification(DateTimeOffset time, string title, string message)
        {
            Time = time;
            Title = title;
            Message = message;
        }

        public int CompareTo(object obj)
        {
            return obj is not Notification other ? 1 : Time.CompareTo(other.Time);
        }
    }
}