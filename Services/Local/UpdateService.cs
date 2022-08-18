using System.Threading.Tasks;
using F1Desktop.Misc;
using NuGet.Versioning;
using Squirrel;
using Squirrel.Sources;

namespace F1Desktop.Services.Local;

public class UpdateService : IDisposable
{
    private readonly UpdateManager _mgr;
    private UpdateInfo _availableUpdate;
    private IAppTools _appTools;

    public string Version { get; private set; } = "DEBUG";
    public bool FirstRun { get; private set; }
    public bool IsPortable => !_mgr.IsInstalledApp;

    public UpdateService()
    {
        var githubSource = new GithubSource("https://github.com/sirdoombox/F1Desktop", string.Empty, false);
        _mgr = new UpdateManager(githubSource);
    }

    public async Task<bool> CheckForUpdate()
    {
        if (IsPortable) return false;
        _availableUpdate = await _mgr.CheckForUpdate();
        return _availableUpdate != null;
    }

    public async Task<bool> Update(Action<int> progress = null)
    {
        if (IsPortable) return false;
        var newVersion = await _mgr.UpdateApp(progress);
        return newVersion != null;
    }

    public void CreateDesktopShortcut() => 
        _appTools?.CreateShortcutsForExecutable(Constants.AppExe, ShortcutLocation.StartMenu | ShortcutLocation.Desktop, false, null, null);

    public void OnAppInstall(SemanticVersion version, IAppTools tools)
    {
        
    }

    public void OnAppUninstall(SemanticVersion version, IAppTools tools)
    {
        RegistryHelper.DeleteKey(Constants.RegistryStartupSubKey, Constants.AppName);
        tools.RemoveShortcutForThisExe();
    }

    public void OnAppRun(SemanticVersion version, IAppTools tools, bool firstRun)
    {
        if (!tools.IsInstalledApp) return;
        Version = $"{version.Major}.{version.Minor}.{version.Patch}";
        FirstRun = firstRun;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _mgr.Dispose();
    }
}