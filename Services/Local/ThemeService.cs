using System.Windows;
using AdonisUI;
using F1Desktop.Services.Base;
using H.NotifyIcon;

namespace F1Desktop.Services.Local;

public class ThemeService : ServiceBase
{
    private TaskbarIcon _icon;
    private GlobalConfigService _config;

    public ThemeService(GlobalConfigService config)
    {
        _config = config;
        SetTheme(_config.UseLightTheme);
        _config.OnPropertyChanged += prop =>
        {
            if (prop != nameof(GlobalConfigService.UseLightTheme)) return;
            SetTheme(_config.UseLightTheme);
        };
    }

    public void SetTheme(bool isLight)
    {
        ResourceLocator.SetColorScheme(Application.Current.Resources,
            isLight ? ResourceLocator.LightColorScheme : ResourceLocator.DarkColorScheme);
        _icon?.UpdateDefaultStyle();
    }

    public void PassIcon(TaskbarIcon icon)
    {
        _icon = icon;
        SetTheme(_config.UseLightTheme);
    }
}