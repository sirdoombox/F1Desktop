using System.Web;
using F1Desktop.Misc;
using F1Desktop.Models.News;
using Stylet;

namespace F1Desktop.Features.News;

public class NewsItemViewModel : PropertyChangedBase
{
    public string Title { get; }
    public string Text { get; }
    public string Url { get; }
    public string Image { get; }
    public string ProviderName { get; }
    public DateTimeOffset Published { get; }
    
    private bool _use24HourClock;
    public bool Use24HourClock
    {
        get => _use24HourClock;
        set => SetAndNotify(ref _use24HourClock, value);
    }

    public NewsItemViewModel(NewsItem item)
    {
        Title = HttpUtility.HtmlDecode(item.Title);;
        Text = HttpUtility.HtmlDecode(item.Content);
        Url = item.Url;
        Image = item.ImageUrl;
        Published = item.Published;
        ProviderName = item.Provider;
    }

    public void OpenArticle() => UrlHelper.Open(Url);
}