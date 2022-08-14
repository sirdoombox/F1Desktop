using F1Desktop.Features.Base;
using F1Desktop.Services.Local;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Settings;

public class SettingsRootViewModel : FeatureBase
{
    private bool _isLight;
    public bool IsLight
    {
        get => _isLight;
        set
        {
            if (!SetAndNotify(ref _isLight, value)) return;
            _config.UseLightTheme = _isLight;
            OnThemeChanged();
        }
    }

    private bool _use24HourClock;
    public bool Use24HourClock
    {
        get => _use24HourClock;
        set
        {
            if (!SetAndNotify(ref _use24HourClock, value)) return;
            _config.Use24HourClock = _use24HourClock;
        }
    }

    public CreditsViewModel Credits { get; }

    private readonly ThemeService _theme;
    private readonly GlobalConfigService _config;

    public SettingsRootViewModel(GlobalConfigService config, ThemeService theme, CreditsViewModel credits)
        : base("Settings", PackIconMaterialKind.Cog, byte.MaxValue)
    {
        _config = config;
        _config.OnPropertyChanged += OnGlobalPropertyChanged;
        _theme = theme;
        Credits = credits;
    }

    private void OnGlobalPropertyChanged(string obj)
    {
        SetAndNotify(ref _use24HourClock, _config.Use24HourClock, nameof(Use24HourClock));
        SetAndNotify(ref _isLight, _config.UseLightTheme, nameof(IsLight));
    }

    protected override async void OnFeatureFirstOpened() => 
        await Credits.LoadCredits();

    private void OnThemeChanged() => 
        _theme.SetTheme(IsLight);

    protected override async void OnFeatureHidden() => await _config.SaveConfig();
}