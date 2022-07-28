using System.Diagnostics;
using F1Desktop.Models.ErgastAPI.Schedule;

namespace F1Desktop.Misc;

public static class UrlHelper
{
    public static void Open(Circuit circuit, byte depth = 15)
    {
        var mapsUrl = $@"https://www.google.com/maps/@{circuit.Location.Lat},{circuit.Location.Long},{depth}z";
        Open(mapsUrl);
    }

    public static void Open(string url)
    {
        var psi = new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        };
        Process.Start(psi);
    }
}