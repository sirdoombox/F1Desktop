namespace F1Desktop.Services.Rss.Providers;

public class RacerRssProvider : IRssProvider
{
    public string Url => "https://racer.com/f1/feed/";
    public string ProviderName => "Racer";
}