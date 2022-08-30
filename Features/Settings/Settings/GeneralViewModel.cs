using F1Desktop.Features.Settings.Settings.Base;
using F1Desktop.Services.Local;

namespace F1Desktop.Features.Settings.Settings;

public class GeneralViewModel : SetingsCategoryViewModelBase
{
    private bool _isLight;
    public bool IsLight
    {
        get => _isLight;
        set => SetAndNotifyWithConfig(ref _isLight, c => c.UseLightTheme, value);
    }

    private bool _use24HourClock;
    public bool Use24HourClock
    {
        get => _use24HourClock;
        set => SetAndNotifyWithConfig(ref _use24HourClock, c => c.Use24HourClock, value);
    }

    private bool _startWithWindows;
    public bool StartWithWindows
    {
        get => _startWithWindows;
        set => SetAndNotifyWithConfig(ref _startWithWindows, c => c.StartWithWindows, value);
    }
    
    private bool _showWindowOnStartup;
    public bool ShowWindowOnStartup
    {
        get => _showWindowOnStartup;
        set => SetAndNotifyWithConfig(ref _showWindowOnStartup, c => c.ShowWindowOnStartup, value);
    }

    public GeneralViewModel(GlobalConfigService config) : base("General", config)
    {
        
    }

    protected sealed override void OnGlobalConfigPropertyChanged(string propName)
    {
        Use24HourClock = Config.Use24HourClock;
        IsLight = Config.UseLightTheme;
        StartWithWindows = Config.StartWithWindows;
        ShowWindowOnStartup = Config.ShowWindowOnStartup;
    }
}