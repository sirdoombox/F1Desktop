using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.News;
using F1Desktop.Models.Resources;
using F1Desktop.Services.Local;
using JetBrains.Annotations;

namespace F1Desktop.Services.Remote;

[UsedImplicitly]
public class NewsRssService
{
    private readonly DataResourceService _resources;
    private IEnumerable<RssProviderData> _providers;

    public NewsRssService(DataResourceService resources)
    {
        _resources = resources;
    }

    public async Task<IEnumerable<NewsItem>> GetNewsAsync()
    {
        _providers ??= await _resources.LoadResourceAsync<List<RssProviderData>>();
        var tasks = new List<Task<IEnumerable<NewsItem>>>();
        foreach (var provider in _providers)
            tasks.Add(Task.Run(() => GetFeed(provider.Url, provider.Name)));
        var results = await Task.WhenAll(tasks);
        return results.SelectMany(x => x);
    }

    public IEnumerable<string> GetProviders()
    {
        _providers ??= _resources.LoadResourceAsync<List<RssProviderData>>().GetAwaiter().GetResult();
        return _providers.Select(x => x.Name);
    }

    private IEnumerable<NewsItem> GetFeed(string url, string name)
    {
        using var reader = XmlReader.Create(url);
        var feed = SyndicationFeed.Load(reader);
        return feed.Items.Select(x => new NewsItem
        {
            Title = x.Title.Text,
            Content = x.Summary.Text.HtmlDecode().TruncateForDisplay(),
            ImageUrl = feed.ImageUrl?.ToString(),
            Provider = name,
            Published = x.PublishDate,
            Url = x.Id ?? x.Links[0].Uri.ToString()
        });
    }
}