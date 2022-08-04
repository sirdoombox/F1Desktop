using System.ServiceModel.Syndication;
using F1Desktop.Models.News;

namespace F1Desktop.Services.Rss.Providers;

public interface IRssProvider
{
    public string Url { get; }
    public string ProviderName { get; }
    
    /// <summary>
    /// Transforms a syndication feed into a parsed collection of NewsItems, default implementation is included.
    /// </summary>
    /// <param name="feed"></param>
    /// <returns></returns>
    public IEnumerable<NewsItem> GetNewsItems(SyndicationFeed feed) =>
        feed.Items
            .Select(x => new NewsItem
            {
                Title = x.Title.Text,
                Content = x.Summary.Text,
                ImageUrl = feed.ImageUrl.ToString(),
                Provider = ProviderName,
                Published = x.PublishDate,
                Url = x.Id
            });
}