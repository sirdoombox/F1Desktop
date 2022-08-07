using F1Desktop.Attributes;
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
    private readonly GlobalConfig _global;
    
    public NewsRootViewModel(ConfigService cfg, NewsRssService rss, GlobalConfig global) 
        : base("News", PackIconMaterialKind.Newspaper, cfg, 3)
    {
        _rss = rss;
        _global = global;
        cfg.SubscribeToConfigChange<GlobalConfig>(OnGlobalConfigChanged);
    }
    
    private void OnGlobalConfigChanged()
    {
        if (NewsItems.Count == 0) return;
        if (_global.Use24HourClock == NewsItems[0].Use24HourClock) return;
        foreach (var race in NewsItems)
            race.Use24HourClock = _global.Use24HourClock;
    }

    protected override async void OnActivationComplete()
    {
        var items = await _rss.GetNewsAsync();
        var newsItems = items.Select(newsItem => new NewsItemViewModel(newsItem)
            {
                Use24HourClock = _global.Use24HourClock
            })
            .OrderByDescending(x => x.Published);
        NewsItems.AddRange(newsItems);
    }
}