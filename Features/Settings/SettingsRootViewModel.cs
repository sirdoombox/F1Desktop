using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
using F1Desktop.Services;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Settings;

public class SettingsRootViewModel : FeatureBaseWithConfig<GlobalConfig>
{
    private bool _isLight;
    public bool IsLight
    {
        get => _isLight;
        set
        {
            if (!SetAndNotifyWithConfig(ref _isLight, x => x.LightTheme, value)) return;
            OnThemeChanged();
        }
    }
    
    public CreditsViewModel Credits { get; }
    
    private readonly ThemeService _theme;
    
    public SettingsRootViewModel(ConfigService configService, ThemeService theme, CreditsViewModel credits) 
        : base("Settings", PackIconMaterialKind.Cog, configService, byte.MaxValue)
    {
        _theme = theme;
        Credits = credits;
    }

    protected override void OnConfigLoaded()
    {
        IsLight = Config.LightTheme;
    }

    private void OnThemeChanged()
    {
        _theme.SetTheme(IsLight);
    }
}