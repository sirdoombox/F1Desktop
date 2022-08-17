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
            if(!SetAndNotify(ref _startWithWindows, value)) return;
            _config.StartWithWindows = _startWithWindows;
        }
    }

    public CreditsViewModel Credits { get; }

    private readonly GlobalConfigService _config;

    public SettingsRootViewModel(GlobalConfigService config, CreditsViewModel credits)
        : base("Settings", PackIconMaterialKind.Cog, byte.MaxValue)
    {
        _config = config;
        _config.OnPropertyChanged += OnGlobalPropertyChanged;
        Credits = credits;
    }

    private void OnGlobalPropertyChanged(string obj)
    {
        SetAndNotify(ref _use24HourClock, _config.Use24HourClock, nameof(Use24HourClock));
        SetAndNotify(ref _isLight, _config.UseLightTheme, nameof(IsLight));
        SetAndNotify(ref _startWithWindows, _config.StartWithWindows, nameof(StartWithWindows));
    }

    protected override async void OnFeatureFirstOpened() => 
        await Credits.LoadCredits();

    protected override async void OnFeatureHidden() => await _config.SaveConfig();
}