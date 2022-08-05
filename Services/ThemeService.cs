using H.NotifyIcon;

namespace F1Desktop.Services;

public class ThemeService
{
    private TaskbarIcon _icon;
    private readonly Func<TaskbarIcon> _tryGetIcon;

    public ThemeService(Func<TaskbarIcon> tryGetIcon)
    {
        _tryGetIcon = tryGetIcon;
        _icon = _tryGetIcon();
    }

    public void ChangeTheme()
    {
        _icon ??= _tryGetIcon();
        
    }
}