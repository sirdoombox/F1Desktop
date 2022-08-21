using System.Threading.Tasks;
using F1Desktop.Misc;
using F1Desktop.Models.Misc;
using NuGet.Versioning;
using Squirrel;
using Squirrel.Sources;

namespace F1Desktop.Services.Local;

public class UpdateService : IDisposable
{
    public bool IsJustUpdated { get; }
    public string Version => _version is null ? "DEBUG" : _version.ToString();
    public bool FirstRun { get; }
    public bool IsPortable => !_mgr.IsInstalledApp;
    public Action<string> OnUpdateAvailable { get; set; }
    
    private IReadOnlyList<string> changelog;
    
    private readonly UpdateManager _mgr;
    private readonly IAppTools _appTools;
    private readonly DataResourceService _dataResource;
    private readonly SemanticVersion _version;
    
    public UpdateService(StartupState startupState, DataResourceService dataResource)
    {
        var githubSource = new GithubSource(Constants.Url.GitHubRepo, string.Empty, false);
        _mgr = new UpdateManager(githubSource);
        _dataResource = dataResource;
        if (IsPortable) return;
        _appTools = startupState.AppTools;
        FirstRun = startupState.FirstRun;
        IsJustUpdated = startupState.JustUpdated;
        _version = startupState.Version;
    }

    public async Task Update()
    {
        if (IsPortable) return;
        var newVersion = await _mgr.UpdateApp();
        if (newVersion == null) return;
        OnUpdateAvailable?.Invoke(newVersion.Version.ToString());
    }

    public async Task<IReadOnlyList<string>> GetChangeLog()
    {
        if (changelog != null) return changelog;
        if (IsPortable)
            changelog = Enumerable.Range(1, 10).Select(x => $"- Debug Change {x}.").ToList();
        else
            changelog = await _dataResource.LoadChangelogForVersion(Version).ToListAsync();
        return changelog;
    }

    public void ApplyUpdate() =>
        UpdateManager.RestartApp(arguments: "--just-updated");

    public void CreateDesktopShortcut() =>
        _appTools?.CreateShortcutsForExecutable(Constants.App.Exe, ShortcutLocation.StartMenu | ShortcutLocation.Desktop,
            false, null, null);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _mgr.Dispose();
    }
}