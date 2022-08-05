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

    public async Task<IEnumerable<NewsItem>> GetNewsAsync()
    {
        var tasks = new List<Task<IEnumerable<NewsItem>>>();
        foreach (var provider in _providers)
        {
            tasks.Add(Task.Run(() => GetFeed(provider)));
        }
        var results = await Task.WhenAll(tasks);
        return results.SelectMany(x => x);
    }

    private IEnumerable<NewsItem> GetFeed(IRssProvider provider)
    {
        var newsItems = new List<NewsItem>();
        using var reader = XmlReader.Create(provider.Url);
        newsItems.AddRange(provider.GetNewsItems(SyndicationFeed.Load(reader)));
        return newsItems;
    }
}