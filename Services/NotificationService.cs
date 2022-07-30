using H.NotifyIcon;

namespace F1Desktop.Services;

public class NotificationService
{
    private readonly TaskbarIcon _icon;
    
    public NotificationService(TaskbarIcon icon)
    {
        _icon = icon;
    }

    public void ShowNotification(string title, string message)
    {
        _icon.ShowNotification(title,message);
    }
}