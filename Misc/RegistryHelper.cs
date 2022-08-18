using Microsoft.Win32;

namespace F1Desktop.Misc;

public static class RegistryHelper
{
    public static void SetKey(string subKey, string key, object value)
    {
        var rk = Registry.CurrentUser.OpenSubKey(subKey, true);
        rk?.SetValue(key, value);
    }

    public static void DeleteKey(string subKey, string key)
    {
        var rk = Registry.CurrentUser.OpenSubKey(subKey, true);
        rk?.DeleteValue(key);
    }
}