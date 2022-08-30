using F1Desktop.Enums;
using F1Desktop.Services.Base;
using F1Desktop.Services.Interfaces;
using H.NotifyIcon;

namespace F1Desktop.Services.Local;

public class NotificationService : ServiceBase, INotificationServiceDebug
{
    public  Dictionary<object, List<ScheduledNotification>> Owners { get; } = new();
    
    private readonly SortedSet<ScheduledNotification> _scheduled = new();
    private bool _notificationShowing;
    
    private TaskbarIcon _icon;
    private readonly GlobalConfigService _config;
    private readonly ITimeService _time;

    public NotificationService(ITimeService time, GlobalConfigService config)
    {
        _time = time;
        _time.RegisterTickCallback(Every.OneSecond, Tick);
        _config = config;
    }

    public bool ShowNotification(string title, Func<string> message, Action onNotificationClicked = null, Action<DateTimeOffset> onShow = null, bool isScheduled = false)
    {
        if (!_config.EnableNotifications) return true;
        if ((_icon is null || _notificationShowing) && !isScheduled)
        {
            ScheduleNotification(this, _time.OffsetNow, title, message, onNotificationClicked);
            return false;
        }
        
        if (onNotificationClicked is not null)
            _icon.TrayBalloonTipClicked += (_, _) => onNotificationClicked();
        void ShowHandler(object s, EventArgs e)
        {
            onShow?.Invoke(_time.OffsetNow);
            _icon.TrayBalloonTipShown -= ShowHandler;
        }
        _icon.TrayBalloonTipShown += ShowHandler;
        _icon.ShowNotification(title, message(), sound: _config.EnableNotificationsSound);
        return true;
    }

    public void ShowNotification(string title, string message, Action onNotificationClicked = null) =>
        ShowNotification(title, () => message, onNotificationClicked);

    public void ScheduleNotification(object owner, DateTimeOffset time, string title, Func<string> message,
        Action onNotificationClicked = null, Action<DateTimeOffset> onShow = null, Func<DateTimeOffset,bool> shouldShow = null)
    {
        var notification = new ScheduledNotification(time, title, message, onNotificationClicked, onShow, shouldShow);
        if (Owners.TryGetValue(owner, out var list))
            list.Add(notification);
        else
            Owners.Add(owner, new List<ScheduledNotification> { notification });
        _scheduled.Add(notification);
    }

    public void ScheduleNotification(object owner, DateTimeOffset time, string title, string message,
        Action onNotificationClicked = null, Action<DateTimeOffset> onShow = null, Func<DateTimeOffset,bool> shouldShow = null) =>
        ScheduleNotification(owner, time, title, () => message, onNotificationClicked, onShow, shouldShow);

    public void CancelNotification(ScheduledNotification scheduledNotification)
    {
        _scheduled.Remove(scheduledNotification);
        List<object> toRemove = new();
        foreach (var owner in Owners)
        {
            owner.Value.Remove(scheduledNotification);
            if(owner.Value.Count <= 0)
                toRemove.Add(owner);
        }

        foreach (var ownerToRemove in toRemove)
            Owners.Remove(ownerToRemove);
    }

    public void CancelAllNotifications(object owner)
    {
        if (!Owners.TryGetValue(owner, out var list)) return;
        foreach (var item in list)
            _scheduled.Remove(item);
        Owners.Remove(owner);
    }

    public void PassIcon(TaskbarIcon icon)
    {
        if (_icon is not null) return;
        _icon = icon;
        if (_icon is null) return;
        _icon.TrayBalloonTipShown += (_, _) => _notificationShowing = true;
        _icon.TrayBalloonTipClosed += (_, _) => _notificationShowing = false;
        _icon.TrayBalloonTipClicked += (_, _) => _notificationShowing = false;
    }

    private void Tick(DateTimeOffset offsetNow)
    {
        if (_notificationShowing) return;
        var first = _scheduled.Min;
        if (first is null) return;
        if (first.Time > offsetNow) return;
        if (first.ShouldShow is not null && !first.ShouldShow.Invoke(offsetNow))
        {
            CancelNotification(first);
            return;
        }
        var isShown = ShowNotification(first.Title, first.Message, first.OnClickedCallback, first.OnShow, true);
        if(isShown)
            CancelNotification(first);
    }

    public class ScheduledNotification : IComparable
    {
        public DateTimeOffset Time { get; }
        public string Title { get; }
        public Func<string> Message { get; }
        public Action OnClickedCallback { get; }
        public Action<DateTimeOffset> OnShow { get; }
        public Func<DateTimeOffset,bool> ShouldShow { get; }

        public ScheduledNotification(DateTimeOffset time, string title, Func<string> message, Action onClickedCallback, 
            Action<DateTimeOffset> onShow, Func<DateTimeOffset,bool> shouldShow = null)
        {
            Time = time;
            Title = title;
            Message = message;
            OnClickedCallback = onClickedCallback;
            OnShow = onShow;
            ShouldShow = shouldShow;
        }

        public int CompareTo(object obj)
        {
            return obj is not ScheduledNotification other ? 1 : Time.CompareTo(other.Time);
        }
    }
}