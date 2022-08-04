using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using F1Desktop.Models.News;
using F1Desktop.Services.Rss.Providers;
using JetBrains.Annotations;

namespace F1Desktop.Services.Rss;

[UsedImplicitly]
public class NewsRssService
{
    private readonly IEnumerable<IRssProvider> _providers;

    public NewsRssService(IEnumerable<IRssProvider> providers)
    {
        _providers = providers;
    }

    public Task<IEnumerable<NewsItem>> GetNewsAsync()
    {
        return Task.Run(GetFeeds);
    }

    private IEnumerable<NewsItem> GetFeeds()
    {
        var feeds = new List<NewsItem>();
        foreach (var provider in _providers)
        {
            using var reader = XmlReader.Create(provider.Url);
            feeds.AddRange(provider.GetNewsItems(SyndicationFeed.Load(reader)));
        }
        return feeds;
    }
}