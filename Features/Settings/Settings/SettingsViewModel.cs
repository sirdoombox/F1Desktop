using System.Threading.Tasks;
using AdonisUI.Controls;
using F1Desktop.Misc;
using F1Desktop.Services.Local;

namespace F1Desktop.Features.Settings.Settings;

public class SettingsViewModel : PropertyChangedBase
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
    
    private bool _showWindowOnStartup;
    public bool ShowWindowOnStartup
    {
        get => _showWindowOnStartup;
        set
        {
            if(!SetAndNotify(ref _showWindowOnStartup, value)) return;
            _config.ShowWindowOnStartup = _showWindowOnStartup;
        }
    }

    private readonly GlobalConfigService _config;

    public SettingsViewModel(GlobalConfigService config)
    {
        _config = config;
        _config.OnPropertyChanged += _ => OnGlobalPropertyChanged();
        OnGlobalPropertyChanged();
    }
    
    public Task ResetToDefault() =>
        MessageBox.Show(MessageBoxModels.ResetToDefault) == MessageBoxResult.Yes 
            ? _config.ResetDefault() 
            : Task.CompletedTask;
    
    private void OnGlobalPropertyChanged()
    {
        Use24HourClock = _config.Use24HourClock;
        IsLight = _config.UseLightTheme;
        StartWithWindows = _config.StartWithWindows;
        ShowWindowOnStartup = _config.ShowWindowOnStartup;
    }
}