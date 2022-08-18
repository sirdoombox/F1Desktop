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

    private bool _startWithWindows;
    public bool StartWithWindows
    {
        get => _startWithWindows;
        set
        {
            if (!SetAndNotify(ref _startWithWindows, value)) return;
            _config.StartWithWindows = _startWithWindows;
        }
    }

    public string Version { get; }

    public CreditsViewModel Credits { get; }

    private readonly GlobalConfigService _config;

    public SettingsRootViewModel(GlobalConfigService config, CreditsViewModel credits, UpdateService update)
        : base("Settings", PackIconMaterialKind.Cog, byte.MaxValue)
    {
        Version = $"Version: {update.Version}";
        Credits = credits;
        _config = config;
        _config.OnPropertyChanged += _ => OnGlobalPropertyChanged();
        OnGlobalPropertyChanged();
    }

    private void OnGlobalPropertyChanged()
    {
        Use24HourClock = _config.Use24HourClock;
        IsLight = _config.UseLightTheme;
        StartWithWindows = _config.StartWithWindows;
    }

    protected override async void OnFeatureFirstOpened() =>
        await Credits.LoadCredits();

    protected override async void OnFeatureHidden() => await _config.SaveConfig();
}