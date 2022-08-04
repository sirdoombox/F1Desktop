using System.ServiceModel.Syndication;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.News;

namespace F1Desktop.Services.Rss.Providers;

public class FIARssProvider : IRssProvider
{
    public string Url => "https://www.fia.com/rss/press-release";
    public string ProviderName => "FIA";

    public IEnumerable<NewsItem> GetNewsItems(SyndicationFeed feed)
    {
        // TODO: Figure out what to do with this and the Formula 1 providers - neither provide publish dates.
        return new List<NewsItem>();
        return feed.Items
            .Where(x => x.Title.Text.ToLowerInvariant().Contains("f1"))
            .Select(x => new NewsItem
            {
                Title = x.Title.Text,
                Content = x.Summary.Text.HtmlDecode().TruncateForDisplay(),
                ImageUrl = "https://cdn.freebiesupply.com/logos/large/2x/fia-logo-png-transparent.png",
                Provider = ProviderName,
                Published = x.PublishDate,
                Url = x.Id
            });
    }
}