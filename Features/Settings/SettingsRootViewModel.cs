using System.Threading.Tasks;
using AdonisUI.Controls;
using F1Desktop.Features.Base;
using F1Desktop.Features.Settings.Changelogs;
using F1Desktop.Features.Settings.Credits;
using F1Desktop.Features.Settings.Settings.Base;
using F1Desktop.Misc;
using F1Desktop.Services.Local;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Settings;

public class SettingsRootViewModel : FeatureBase
{
    public string Version { get; }

    public List<SetingsCategoryViewModelBase> Categories { get; }
    
    public CreditsViewModel Credits { get; }
    public ChangelogsViewModel Changelogs { get; }

    private readonly UpdateService _update;
    private readonly NotificationService _notification;
    private readonly GlobalConfigService _config;
    private readonly IWindowManager _windowManager;

    public SettingsRootViewModel(GlobalConfigService config, UpdateService update, NotificationService notification, 
        IEnumerable<SetingsCategoryViewModelBase> categories, 
        CreditsViewModel credits, ChangelogsViewModel changelogs, IWindowManager windowManager)
        : base("Settings", PackIconMaterialKind.Cog, byte.MaxValue)
    {
        _config = config;
        _update = update;
        _notification = notification;
        _windowManager = windowManager;

        Categories = categories.ToList();
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
            _notification.ShowNotification("Up To Date", "F1 Desktop is currently up to date.", null);
    }

    public void OpenCreditsWindow() => _windowManager.ShowWindow(Credits);

    public void OpenChangelogWindow() => _windowManager.ShowWindow(Changelogs);

    public Task ResetToDefault() =>
        MessageBox.Show(MessageBoxModels.ResetToDefault) == MessageBoxResult.Yes 
            ? _config.ResetDefault() 
            : Task.CompletedTask;
    
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