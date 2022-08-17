using System.Threading.Tasks;
using Squirrel;
using Squirrel.Sources;

namespace F1Desktop.Services.Local;

public class UpdateService : IDisposable
{
    private readonly UpdateManager _mgr;
    private bool IsPortable => !_mgr.IsInstalledApp;
    private UpdateInfo _availableUpdate = null;
    
    public string Version { get; }
    
    public UpdateService(string version)
    {
        Version = version;
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

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _mgr.Dispose();
    }
}