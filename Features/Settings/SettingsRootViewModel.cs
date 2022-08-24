using System.Threading.Tasks;
using AdonisUI.Controls;
using F1Desktop.Features.Base;
using F1Desktop.Misc;
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

    public string Version { get; }

    public CreditsViewModel Credits { get; }

    private readonly GlobalConfigService _config;
    private readonly UpdateService _update;
    private readonly NotificationService _notification;

    public SettingsRootViewModel(GlobalConfigService config, CreditsViewModel credits, UpdateService update, NotificationService notification)
        : base("Settings", PackIconMaterialKind.Cog, byte.MaxValue)
    {
        _config = config;
        _update = update;
        _notification = notification;
        Credits = credits;
        Version = _update.Version;
        _config.OnPropertyChanged += _ => OnGlobalPropertyChanged();
        OnGlobalPropertyChanged();
    }

    public void OpenGithubRepo() => 
        UrlHelper.Open(Constants.Url.GitHubRepo);

    public async Task OpenChangelog()
    {
        var changeLog = MessageBoxModels.ChangeLog;
        changeLog.Caption = $"Changelog for version: v{_update.Version}";
        changeLog.Text = string.Join("\r\n", await _update.GetChangeLog());
        MessageBox.Show(changeLog);
    }

    public async Task CheckForUpdate()
    {
        var updateAvailable = await _update.Update();
        if(!updateAvailable)
            _notification.ShowNotification("Up To Date", "F1 Desktop is currently up to date.");
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

    protected override async void OnFeatureFirstOpened() =>
        await Credits.LoadCredits();

    protected override async void OnFeatureHidden() => 
        await _config.SaveConfig();
}