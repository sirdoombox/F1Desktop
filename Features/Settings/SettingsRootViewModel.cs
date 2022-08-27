using System.Threading.Tasks;
using AdonisUI.Controls;
using F1Desktop.Features.Base;
using F1Desktop.Features.Settings.Changelogs;
using F1Desktop.Features.Settings.Credits;
using F1Desktop.Features.Settings.Settings;
using F1Desktop.Misc;
using F1Desktop.Services.Local;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Settings;

public class SettingsRootViewModel : FeatureBase
{
    public string Version { get; }

    public SettingsViewModel Settings { get; }
    public CreditsViewModel Credits { get; }
    public ChangelogsViewModel Changelogs { get; }

    private readonly UpdateService _update;
    private readonly NotificationService _notification;
    private readonly GlobalConfigService _config;
    private readonly IWindowManager _windowManager;

    public SettingsRootViewModel(GlobalConfigService config, UpdateService update, NotificationService notification, 
        SettingsViewModel settings, CreditsViewModel credits, ChangelogsViewModel changelogs, IWindowManager windowManager)
        : base("Settings", PackIconMaterialKind.Cog, byte.MaxValue)
    {
        _config = config;
        _update = update;
        _notification = notification;
        _windowManager = windowManager;

        Settings = settings;
        Credits = credits;
        Changelogs = changelogs;
            
        Version = _update.Version;
    }

    public void OpenGithubRepo() => 
        UrlHelper.Open(Constants.Url.GitHubRepo);

    public async Task CheckForUpdate()
    {
        var updateAvailable = await _update.Update();
        if(!updateAvailable)
            _notification.ShowNotification("Up To Date", "F1 Desktop is currently up to date.");
    }

    public void OpenCreditsWindow() => _windowManager.ShowWindow(Credits);

    public void OpenChangelogWindow() => _windowManager.ShowWindow(Changelogs);

    protected override async void OnFeatureFirstOpened()
    {
        var credits = Credits.LoadCredits();
        var change = Changelogs.LoadChangelog();
        await credits;
        await change;
    }

    protected override async void OnFeatureHidden() => 
        await _config.SaveConfig();
}