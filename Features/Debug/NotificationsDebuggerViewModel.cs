using F1Desktop.Features.Debug.Base;
using F1Desktop.Services.Interfaces;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Debug;

public class NotificationsDebuggerViewModel : DebugFeatureBase
{
    public BindableCollection<NotificationViewModel> Notifications { get; } = new();

    private readonly INotificationServiceDebug _notification;
    private readonly ITimeServiceDebug _time;

    public NotificationsDebuggerViewModel(INotificationServiceDebug notification, ITimeServiceDebug time) 
        : base("Notifications", PackIconMaterialKind.Alert)
    {
        _notification = notification;
        _time = time;
    }

    public void Update()
    {
        Notifications.Clear();
        foreach (var owner in _notification.Owners)
        foreach (var registered in owner.Value)
        {
            Notifications.Add(new NotificationViewModel
            {
                OwnerType = owner.Key.GetType(),
                ScheduledAt = registered.Time,
                Title = registered.Title,
                Message = registered.Message(),
                ShouldShow = registered.ShouldShow(_time.OffsetNow)
            });
        }
    }

    public class NotificationViewModel
    {
        public Type OwnerType { get; init; }
        public DateTimeOffset ScheduledAt { get; init; }
        public string Title { get; init; }
        public string Message { get; init; }
        public bool ShouldShow { get; init; }
    }
}