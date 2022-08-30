using F1Desktop.Services.Local;
using H.NotifyIcon;

namespace F1Desktop.Services.Interfaces;

public interface INotificationServiceDebug : INotificationService
{
    public Dictionary<object, List<NotificationService.ScheduledNotification>> Owners { get; }
}