using System.IO;
using System.Reflection;
using F1Desktop.Misc;
using F1Desktop.Services.Base;

namespace F1Desktop.Services.Local;

public class RegistryService : ServiceBase
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
            var root = PathHelper.MoveUp(assemblyLocation, 2);
            var exePath = Path.Combine(root, $"{Constants.App.Name}.exe");
            RegistryHelper.SetKey(Constants.Misc.RegistryStartupSubKey, Constants.App.Name, exePath);
            return;
        }

        RegistryHelper.DeleteKey(Constants.Misc.RegistryStartupSubKey, Constants.App.Name);
    }
}