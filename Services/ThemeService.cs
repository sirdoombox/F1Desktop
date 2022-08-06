using System.Windows;
using AdonisUI;
using F1Desktop.Models.Config;
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

    public void SetTheme(bool isLight)
    {
        _icon ??= _tryGetIcon();
        ResourceLocator.SetColorScheme(Application.Current.Resources, 
            isLight ? ResourceLocator.LightColorScheme : ResourceLocator.DarkColorScheme);
        _icon?.UpdateDefaultStyle();
    }
}