using H.NotifyIcon;

namespace F1Desktop.Services.Local;

public class NotificationService
{
    private TaskbarIcon _icon;
    private readonly Func<TaskbarIcon> _tryGetIcon;
    private readonly SortedSet<Notification> _scheduled = new();
    private readonly Dictionary<object, List<Notification>> _owners = new();
    private bool _notificationShowing;

    public NotificationService(Func<TaskbarIcon> tryGetIcon, TickService tickService)
    {
        tickService.OneSecond += Tick;
        _tryGetIcon = tryGetIcon;
    }

    public void ShowNotification(string title, string message, Action onNotificationClicked = null)
    {
        if (!TrySetupIcon() || _notificationShowing)
        {
            ScheduleNotification(this, DateTimeOffset.Now, title, message);
            return;
        }

        if (onNotificationClicked is not null)
            _icon.TrayBalloonTipClicked += (_, _) => onNotificationClicked();
        
        _icon.ShowNotification(title, message);
    }

    private bool TrySetupIcon()
    {
        if (_icon is not null) return true;
        _icon = _tryGetIcon();
        if (_icon is null) return false;
        _icon.TrayBalloonTipShown += (_, _) => _notificationShowing = true;
        _icon.TrayBalloonTipClosed += (_, _) => _notificationShowing = false;
        _icon.TrayBalloonTipClicked += (_, _) => _notificationShowing = false;
        return true;
    }

    public void ScheduleNotification(object owner, DateTimeOffset time, string title, string message)
    {
        var notification = new Notification(time, title, message);
        if (_owners.TryGetValue(owner, out var list))
            list.Add(notification);
        else
            _owners.Add(owner, new List<Notification> { notification });
        _scheduled.Add(notification);
    }

    public void CancelNotification(Notification notification)
    {
        _scheduled.Remove(notification);
        foreach (var owner in _owners)
            owner.Value.Remove(notification);
    }

    public void CancelAllNotifications(object owner)
    {
        if (!_owners.TryGetValue(owner, out var list)) return;
        foreach (var item in list)
            _scheduled.Remove(item);
        _owners.Remove(owner);
    }

    private void Tick()
    {
        if (_notificationShowing) return;
        var first = _scheduled.Min;
        if (first is null) return;
        if (first.Time > DateTimeOffset.Now) return;
        ShowNotification(first.Title, first.Message);
        CancelNotification(first);
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