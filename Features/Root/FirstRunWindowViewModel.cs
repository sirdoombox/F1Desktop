using System.Windows;
using F1Desktop.Services.Local;

namespace F1Desktop.Features.Root;

public class FirstRunWindowViewModel : Screen
{
    private bool _useLightTheme;
    public bool UseLightTheme
    {
        get => _useLightTheme;
        set
        {
            if(!SetAndNotify(ref _useLightTheme, value)) return;
            _config.UseLightTheme = _useLightTheme;
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
    
    private bool _use24HourClock;
    public bool Use24HourClock
    {
        get => _use24HourClock;
        set
        {
            if(!SetAndNotify(ref _use24HourClock, value)) return;
            _config.Use24HourClock = _use24HourClock;
        }
    }

    private bool _createShortcut;
    public bool CreateShortcut
    {
        get => _createShortcut;
        set => SetAndNotify(ref _createShortcut, value);
    }

    public Action OnFirstRunClosed { get; set; }

    private readonly UpdateService _update;
    private readonly GlobalConfigService _config;

    public FirstRunWindowViewModel(UpdateService update, GlobalConfigService config)
    {
        _update = update;
        _config = config;
        _config.OnPropertyChanged += _ => LoadConfig();
        
        CreateShortcut = true;
        LoadConfig();
    }

    private void LoadConfig()
    {
        StartWithWindows = _config.StartWithWindows;
        Use24HourClock = _config.Use24HourClock;
        UseLightTheme = _config.UseLightTheme;
    }

    public async void Accept()
    {
        if(CreateShortcut)
            _update.CreateDesktopShortcut();
        await _config.SaveConfig();
        ((Window)View).Close();
        OnFirstRunClosed?.Invoke();
    }
    
    public void OnDeactivated(object sender, EventArgs e)
    {
        var window = (Window)sender;
        window.Topmost = true;
        window.Activate();
    }
}