using System.Windows;
using AdonisUI;
using F1Desktop.Services.Base;
using H.NotifyIcon;

namespace F1Desktop.Services.Local;

public class ThemeService : ServiceBase
{
    private TaskbarIcon _icon;
    private readonly Func<TaskbarIcon> _tryGetIcon;

    public ThemeService(Func<TaskbarIcon> tryGetIcon, GlobalConfigService config)
    {
        _tryGetIcon = tryGetIcon;
        _icon = _tryGetIcon();
        SetTheme(config.UseLightTheme);
        config.OnPropertyChanged += prop =>
        {
            if (prop != nameof(GlobalConfigService.UseLightTheme)) return;
            SetTheme(config.UseLightTheme);
        };
    }

    public void SetTheme(bool isLight)
    {
        _icon ??= _tryGetIcon();
        ResourceLocator.SetColorScheme(Application.Current.Resources,
            isLight ? ResourceLocator.LightColorScheme : ResourceLocator.DarkColorScheme);
        _icon?.UpdateDefaultStyle();
    }
}