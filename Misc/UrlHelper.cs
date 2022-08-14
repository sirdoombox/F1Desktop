using System.Diagnostics;
using F1Desktop.Models.ErgastAPI.Schedule;

namespace F1Desktop.Misc;

public static class UrlHelper
{
    public static void OpenMap(Circuit circuit, byte depth = 15)
    {
        var mapsUrl = $@"https://www.google.com/maps/@{circuit.Location.Lat},{circuit.Location.Long},{depth}z/data=!3m1!1e3";
        Open(mapsUrl);
    }

    public static void OpenWeather(string raceName)
    {
        var urlName = raceName.ToLowerInvariant().Replace(' ', '-');
        Open($"https://trackweather.live/formula1/{urlName}");
    }

    public static void Open(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return;
        var psi = new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        };
        Process.Start(psi);
    }
}