using F1Desktop.Attributes;

namespace F1Desktop.Models.Resources;

[Filename("RssProviders.json")]
public class RssProviderData
{
    public string Name { get; set; }
    public string Url { get; set; }
}