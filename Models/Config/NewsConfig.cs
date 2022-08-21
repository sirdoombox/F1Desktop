using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.Config;

[Filename("News.cfg")]
public sealed class NewsConfig : ConfigBase
{
    public Dictionary<string, bool> Providers { get; set; }
    public int MaxArticles { get; set; }
    public int MaxDays { get; set; }

    public NewsConfig()
    {
        Providers = new Dictionary<string, bool>();
        Default();
    }

    public override void Default()
    {
        MaxArticles = 25;
        MaxDays = 7;
    }
}