#if RELEASE
using System.Reflection;
using F1Desktop.Misc;
using Microsoft.Win32;
#endif

namespace F1Desktop.Services.Local;

public class RegistryService
{
    public RegistryService(GlobalConfigService config)
    {
        config.OnPropertyChanged += _ => SetKey(config.StartWithWindows);
        SetKey(config.StartWithWindows);
    }

    private void SetKey(bool startWithWindows)
    {
        // Prevent adding the debug version to startup.
#if RELEASE
           var rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        if(startWithWindows)
            rk.SetValue(Constants.AppName, Assembly.GetEntryAssembly().Location);
        else
            rk.DeleteValue(Constants.AppName);     
#endif
    }
}