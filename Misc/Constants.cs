using System.Collections.Generic;

namespace F1Desktop.Misc;

public static class Constants
{
    public const string AppName = "F1Desktop";

    public static readonly IReadOnlyDictionary<string, string> BaseNewsFeeds = new Dictionary<string, string>
    {
        { "Formula 1", "https://www.formula1.com/content/fom-website/en/latest/all.xml" },
        { "WTF1", "https://wtf1.com/feed/" },
        { "Racer", "https://racer.com/f1/feed/" }
    };
}