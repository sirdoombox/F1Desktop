using System.ServiceModel.Syndication;
using System.Web;
using F1Desktop.Models.News;
using Stylet;

namespace F1Desktop.Features.News;

public class NewsItemViewModel : PropertyChangedBase
{
    public string Title { get; }
    public string Text { get; }
    public string Url { get; }
    public string Image { get; }
    public DateTimeOffset Published { get; }

    public NewsItemViewModel(NewsItem item)
    {
        Title = HttpUtility.HtmlDecode(item.Title);;
        Text = HttpUtility.HtmlDecode(item.Content);
        Url = item.Url;
        Image = item.ImageUrl;
        Published = item.Published;
    }
}