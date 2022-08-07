using System.ServiceModel.Syndication;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.News;

namespace F1Desktop.Services.Rss.Providers;

public class AutosportRssProvider : IRssProvider
{
    public string Url => "https://www.autosport.com/rss/f1/news/";
    public string ProviderName => "Autosport";
    
    public IEnumerable<NewsItem> GetNewsItems(SyndicationFeed feed) =>
        feed.Items
            .Select(x => new NewsItem
            {
                Title = x.Title.Text,
                Content = x.Summary.Text.HtmlDecode().TruncateForDisplay(),
                ImageUrl = null,
                Provider = ProviderName,
                Published = x.PublishDate,
                Url = x.Links[0].Uri.ToString()
            });
}