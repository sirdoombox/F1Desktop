using System.ServiceModel.Syndication;
using F1Desktop.Models.News;

namespace F1Desktop.Services.Rss.Providers;

public class Formula1RssProvider : IRssProvider
{
    public string Url => "https://www.formula1.com/content/fom-website/en/latest/all.xml";
    public string ProviderName => "Formula 1";

    // TODO: Return empty for now, this RSS provider doesn't supply publish dates which makes ordering it a headache...
    public IEnumerable<NewsItem> GetNewsItems(SyndicationFeed feed) => new List<NewsItem>();
}