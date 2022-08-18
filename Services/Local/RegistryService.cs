using System.IO;
using System.Reflection;
using F1Desktop.Misc;

namespace F1Desktop.Services.Local;

public class RegistryService
{
    private readonly GlobalConfigService _config;
    private readonly UpdateService _update;

    public RegistryService(GlobalConfigService config, UpdateService update)
    {
        _config = config;
        _update = update;
        _config.OnPropertyChanged += GlobalPropertyChanged;
        GlobalPropertyChanged(nameof(GlobalConfigService.StartWithWindows));
    }

    private void GlobalPropertyChanged(string obj)
    {
        if (_update.IsPortable) return;
        if (obj != nameof(GlobalConfigService.StartWithWindows)) return;
        if (_config.StartWithWindows)
        {
            var assemblyLocation = Assembly.GetEntryAssembly()?.Location;
            if (assemblyLocation is null) return;
            var root = Path.GetDirectoryName(Path.GetDirectoryName(assemblyLocation));
            var exePath = Path.Combine(root, $"{Constants.AppName}.exe");
            RegistryHelper.SetKey(Constants.RegistryStartupSubKey, Constants.AppName, exePath);
            return;
        }
        RegistryHelper.DeleteKey(Constants.RegistryStartupSubKey, Constants.AppName);
    }
}