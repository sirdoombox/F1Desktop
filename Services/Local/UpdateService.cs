using System.Threading.Tasks;
using F1Desktop.Misc;
using NuGet.Versioning;
using Squirrel;
using Squirrel.Sources;

namespace F1Desktop.Services.Local;

public class UpdateService : IDisposable
{
    private readonly UpdateManager _mgr;
    private IAppTools _appTools;

    public string Version { get; private set; } = "DEBUG";
    public bool FirstRun { get; private set; }
    public bool IsPortable => !_mgr.IsInstalledApp;

    public Action<string> OnUpdateAvailable { get; set; }

    public bool IsJustUpdated { get; set; }
    
    public IReadOnlyList<string> Changelog { get; private set; }

    public UpdateService()
    {
        var githubSource = new GithubSource(Constants.Url.GitHubRepo, string.Empty, false);
        _mgr = new UpdateManager(githubSource);
    }

    public async Task Update()
    {
        if (IsPortable) return;
        var newVersion = await _mgr.UpdateApp();
        if (newVersion == null) return;
        OnUpdateAvailable?.Invoke(newVersion.Version.ToString());
    }

    public async Task SetChangeLog(DataResourceService _data)
    {
        if (IsPortable)
        {
            Changelog = Enumerable.Range(1, 10).Select(x => $"- Debug Change {x}.").ToList();
            return;
        }
        await _data.LoadChangelogForVersion(Version).ToListAsync();
    }

    public void ApplyUpdate() =>
        UpdateManager.RestartApp(arguments: "--just-updated");

    public void CreateDesktopShortcut() =>
        _appTools?.CreateShortcutsForExecutable(Constants.App.Exe, ShortcutLocation.StartMenu | ShortcutLocation.Desktop,
            false, null, null);

    public void OnAppUninstall(SemanticVersion version, IAppTools tools)
    {
        RegistryHelper.DeleteKey(Constants.Misc.RegistryStartupSubKey, Constants.App.Name);
        tools.RemoveShortcutForThisExe();
    }

    public void OnAppInstall(SemanticVersion version, IAppTools tools) =>
        tools.CreateShortcutForThisExe(ShortcutLocation.StartMenu);

    public void OnAppRun(SemanticVersion version, IAppTools tools, bool firstRun)
    {
        if (!tools.IsInstalledApp) return;
        _appTools = tools;
        Version = version.ToString();
        FirstRun = firstRun;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _mgr.Dispose();
    }
}