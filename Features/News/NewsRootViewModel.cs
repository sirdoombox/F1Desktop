using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
using F1Desktop.Services;
using F1Desktop.Services.Rss;
using JetBrains.Annotations;
using MahApps.Metro.IconPacks;
using Stylet;

namespace F1Desktop.Features.News;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class NewsRootViewModel : FeatureBaseWithConfig<NewsConfig>
{
    public BindableCollection<NewsItemViewModel> NewsItems { get; } = new();

    private readonly NewsRssService _rss;
    
    public NewsRootViewModel(ConfigService cfg, NewsRssService rss) 
        : base("News", PackIconMaterialKind.Newspaper, cfg, 3)
    {
        _rss = rss;
    }

    protected override async void OnActivationComplete()
    {
        var items = await _rss.GetNewsAsync();
        var newsItems = items.Select(newsItem => new NewsItemViewModel(newsItem))
            .OrderByDescending(x => x.Published);
        NewsItems.AddRange(newsItems);
    }
}