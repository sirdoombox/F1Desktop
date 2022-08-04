using System.ServiceModel.Syndication;
using System.Web;
using Stylet;

namespace F1Desktop.Features.News;

public class NewsItemViewModel : PropertyChangedBase
{
    public string Title { get; }
    public string Text { get; }
    public string Url { get; }
    public string Image { get; }
    public DateTimeOffset Published { get; }

    public NewsItemViewModel(SyndicationItem item, string logoUrl)
    {
        Title = HttpUtility.HtmlDecode(item.Title.Text);;
        Text = HttpUtility.HtmlDecode(item.Summary.Text);
        Url = item.Id;
        Image = logoUrl;
        Published = item.PublishDate;
    }
}