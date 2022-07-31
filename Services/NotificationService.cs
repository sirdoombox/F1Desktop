using System;
using H.NotifyIcon;

namespace F1Desktop.Services;

public class NotificationService
{
    private TaskbarIcon _icon;
    private readonly Func<TaskbarIcon> _tryGetIcon;
    
    public NotificationService(Func<TaskbarIcon> tryGetIcon)
    {
        _tryGetIcon = tryGetIcon;
    }

    public void ShowNotification(string title, string message)
    {
        _icon ??= _tryGetIcon();
        _icon?.ShowNotification(title,message);
    }
}