﻿using F1Desktop.Enums;
using F1Desktop.Services.Base;
using F1Desktop.Services.Interfaces;
using H.NotifyIcon;

namespace F1Desktop.Services.Local;

public class NotificationService : ServiceBase
{
    private readonly Func<TaskbarIcon> _tryGetIcon;
    private readonly SortedSet<ScheduledNotification> _scheduled = new();
    private readonly Dictionary<object, List<ScheduledNotification>> _owners = new();
    private bool _notificationShowing;
    
    private TaskbarIcon _icon;
    private readonly GlobalConfigService _config;
    private readonly ITimeService _time;

    public NotificationService(Func<TaskbarIcon> tryGetIcon, ITimeService time, GlobalConfigService config)
    {
        _time = time;
        _time.RegisterTickCallback(Every.OneSecond, Tick);
        _tryGetIcon = tryGetIcon;
        _config = config;
    }

    public void ShowNotification(string title, Func<string> message, Action onNotificationClicked = null, Action<DateTimeOffset> onShow = null)
    {
        if (!_config.EnableNotifications) return;
        if (!TrySetupIcon() || _notificationShowing)
        {
            ScheduleNotification(this, _time.OffsetNow, title, message, onNotificationClicked);
            return;
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
        
    }

    public void ShowNotification(string title, string message, Action onNotificationClicked = null) =>
        ShowNotification(title, () => message, onNotificationClicked);

    public void ScheduleNotification(object owner, DateTimeOffset time, string title, Func<string> message,
        Action onNotificationClicked = null, Action<DateTimeOffset> onShow = null, Func<DateTimeOffset,bool> shouldShow = null)
    {
        var notification = new ScheduledNotification(time, title, message, onNotificationClicked, onShow, shouldShow);
        if (_owners.TryGetValue(owner, out var list))
            list.Add(notification);
        else
            _owners.Add(owner, new List<ScheduledNotification> { notification });
        _scheduled.Add(notification);
    }

    public void ScheduleNotification(object owner, DateTimeOffset time, string title, string message,
        Action onNotificationClicked = null, Action<DateTimeOffset> onShow = null, Func<DateTimeOffset,bool> shouldShow = null) =>
        ScheduleNotification(owner, time, title, () => message, onNotificationClicked, onShow, shouldShow);

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

    private void Tick(DateTimeOffset offsetNow)
    {
        if (_notificationShowing) return;
        var first = _scheduled.Min;
        if (first is null) return;
        if (first.Time > offsetNow) return;
        if (first.ShouldShow?.Invoke(offsetNow) == true)
            ShowNotification(first.Title, first.Message, first.OnClickedCallback, first.OnShow);
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