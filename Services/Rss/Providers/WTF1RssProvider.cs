namespace F1Desktop.Services.Rss.Providers;

public class WTF1RssProvider : IRssProvider
{
    public string Url => "https://wtf1.com/feed/";
    public string ProviderName => "WTF1";
}