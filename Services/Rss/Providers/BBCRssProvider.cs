namespace F1Desktop.Services.Rss.Providers;

public class BBCRssProvider : IRssProvider
{
    public string Url => "https://bbc.com/sport/formula1/rss.xml";
    public string ProviderName => "BBC Sport";
}