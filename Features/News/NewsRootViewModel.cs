using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
using F1Desktop.Services;
using JetBrains.Annotations;
using Stylet;

namespace F1Desktop.Features.News;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class NewsRootViewModel : FeatureRootBase<NewsConfig>
{
    public BindableCollection<NewsItemViewModel> NewsItems { get; } = new();

    private readonly NewsRssService _rss;
    
    public NewsRootViewModel(ConfigService cfg, NewsRssService rss) : base(cfg)
    {
        _rss = rss;
    }

    protected override async void OnActivationComplete()
    {
        var feeds = await _rss.GetFeedsAsync();
        var newsItems = feeds.SelectMany(feed =>
            feed.Items.Select(item => new NewsItemViewModel(item, feed.ImageUrl.ToString())))
            .OrderByDescending(x => x.Published);
        NewsItems.AddRange(newsItems);
    }
}