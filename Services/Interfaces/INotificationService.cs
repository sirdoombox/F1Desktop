using F1Desktop.Services.Local;

namespace F1Desktop.Services.Interfaces;

public interface INotificationService
{
    public bool ShowNotification(string title, Func<string> message, Action onNotificationClicked = null,
        Action<DateTimeOffset> onShow = null, bool isScheduled = false);

    public void ShowNotification(string title, string message, Action onNotificationClicked = null);

    public void ScheduleNotification(object owner, DateTimeOffset time, string title, Func<string> message,
        Action onNotificationClicked = null, Action<DateTimeOffset> onShow = null, Func<DateTimeOffset, bool> shouldShow = null);

    public void ScheduleNotification(object owner, DateTimeOffset time, string title, string message,
        Action onNotificationClicked = null, Action<DateTimeOffset> onShow = null, Func<DateTimeOffset, bool> shouldShow = null);

    public void CancelNotification(NotificationService.ScheduledNotification scheduledNotification);

    public void CancelAllNotifications(object owner);
}