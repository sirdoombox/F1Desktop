using System.Windows;

namespace F1Desktop.Misc.Extensions;

public static class WindowExtensions
{
    public static void TryClose(this Window window)
    {
        try
        {
            window.Close();
        }
        catch
        {
            // Ignored
        }
    }
}