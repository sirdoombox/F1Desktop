using F1Desktop.Services.Base;
using H.NotifyIcon;

namespace F1Desktop.Services.Local;

public class NotificationService : ServiceBase
{
    private TaskbarIcon _icon;
    private readonly Func<TaskbarIcon> _tryGetIcon;
    private readonly SortedSet<ScheduledNotification> _scheduled = new();
    private readonly Dictionary<object, List<ScheduledNotification>> _owners = new();
    private bool _notificationShowing;
    private readonly GlobalConfigService _config;

    public NotificationService(Func<TaskbarIcon> tryGetIcon, TickService tickService, GlobalConfigService config)
    {
        tickService.OneSecond += Tick;
        _tryGetIcon = tryGetIcon;
        _config = config;
    }

    public void ShowNotification(string title, Func<string> message, Action onNotificationClicked = null)
    {
        if (!_config.EnableNotifications) return;
        if (!TrySetupIcon() || _notificationShowing)
        {
            ScheduleNotification(this, DateTimeOffset.Now, title, message, onNotificationClicked);
            return;
        }

        if (onNotificationClicked is not null)
            _icon.TrayBalloonTipClicked += (_, _) => onNotificationClicked();

        _icon.ShowNotification(title, message(), sound: _config.EnableNotificationsSound);
    }

    public void ShowNotification(string title, string message, Action onNotificationClicked = null) =>
        ShowNotification(title, () => message, onNotificationClicked);

    public void ScheduleNotification(object owner, DateTimeOffset time, string title, Func<string> message,
        Action onNotificationClicked = null, Func<bool> shouldShow = null)
    {
        var notification = new ScheduledNotification(time, title, message, onNotificationClicked, shouldShow);
        if (_owners.TryGetValue(owner, out var list))
            list.Add(notification);
        else
            _owners.Add(owner, new List<ScheduledNotification> { notification });
        _scheduled.Add(notification);
    }

    public void ScheduleNotification(object owner, DateTimeOffset time, string title, string message,
        Action onNotificationClicked = null, Func<bool> shouldShow = null) =>
        ScheduleNotification(owner, time, title, () => message, onNotificationClicked, shouldShow);

    public void CancelNotification(ScheduledNotification scheduledNotification)
    {
        _scheduled.Remove(scheduledNotification);
        foreach (var owner in _owners)
            owner.Value.Remove(scheduledNotification);
    }

    public void CancelAllNotifications(object owner)
    {
        if (!_owners.TryGetValue(owner, out var list)) return;
        foreach (var item in list)
            _scheduled.Remove(item);
        _owners.Remove(owner);
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

    private void Tick()
    {
        if (_notificationShowing) return;
        var first = _scheduled.Min;
        if (first is null) return;
        if (first.Time > DateTimeOffset.Now) return;
        if (first.ShouldShow?.Invoke() == true)
            ShowNotification(first.Title, first.Message(), first.OnClickedCallback);
        CancelNotification(first);
    }

    public class ScheduledNotification : IComparable
    {
        public DateTimeOffset Time { get; }
        public string Title { get; }
        public Func<string> Message { get; }
        public Action OnClickedCallback { get; }
        public Func<bool> ShouldShow { get; }

        public ScheduledNotification(DateTimeOffset time, string title, Func<string> message, Action onClickedCallback,
            Func<bool> shouldShow = null)
        {
            Time = time;
            Title = title;
            Message = message;
            OnClickedCallback = onClickedCallback;
            ShouldShow = shouldShow;
        }

        public int CompareTo(object obj)
        {
            return obj is not ScheduledNotification other ? 1 : Time.CompareTo(other.Time);
        }
    }
}