using System.Windows;
using AdonisUI;
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

    private bool _isDark = true;
    
    public void ChangeTheme()
    {
        _icon ??= _tryGetIcon();
        _isDark = !_isDark;
        ResourceLocator.SetColorScheme(Application.Current.Resources, 
            _isDark ? ResourceLocator.DarkColorScheme : ResourceLocator.LightColorScheme);
        _icon.UpdateDefaultStyle();
    }
}