using System.ComponentModel;
using System.Windows.Data;
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
    public BindableCollection<ProviderViewModel> Providers { get; } = new();

    private bool _isNewsItemsUnavailable;

    public bool IsNewsItemsUnavailable
    {
        get => _isNewsItemsUnavailable;
        set => SetAndNotify(ref _isNewsItemsUnavailable, value);
    }

    private readonly NewsRssService _rss;
    private readonly GlobalConfig _global;
    private readonly ICollectionView _newsItemsFilter;

    public NewsRootViewModel(ConfigService cfg, NewsRssService rss, GlobalConfig global)
        : base("News", PackIconMaterialKind.Newspaper, cfg, 3)
    {
        _rss = rss;
        _global = global;
        _newsItemsFilter = CollectionViewSource.GetDefaultView(NewsItems);
        _newsItemsFilter.SortDescriptions.Clear();
        _newsItemsFilter.SortDescriptions.Add(new SortDescription("Published", ListSortDirection.Descending));
        _newsItemsFilter.Filter = o =>
        {
            var newsItem = (NewsItemViewModel)o;
            return Providers.First(x => x.ProviderName == newsItem.ProviderName).IsEnabled;
        };
        cfg.SubscribeToConfigChange<GlobalConfig>(OnGlobalConfigChanged);
        NewsItems.CollectionChanged += (_, _) => IsNewsItemsUnavailable = NewsItems.Count <= 0;
        Providers.AddRange(rss.GetProviders().Select(x => new ProviderViewModel(x, true)));
        foreach (var provider in Providers)
            provider.PropertyChanged += (_, _) => _newsItemsFilter.Refresh();
    }

    public async void RefreshNews()
    {
        NewsItems.Clear();
        var items = await _rss.GetNewsAsync();
        var newsItems = items.Select(newsItem => new NewsItemViewModel(newsItem)
            {
                Use24HourClock = _global.Use24HourClock
            })
            .OrderByDescending(x => x.Published);
        NewsItems.AddRange(newsItems);
    }

    private void OnGlobalConfigChanged()
    {
        if (NewsItems.Count == 0) return;
        if (_global.Use24HourClock == NewsItems[0].Use24HourClock) return;
        foreach (var race in NewsItems)
            race.Use24HourClock = _global.Use24HourClock;
    }

    protected override void OnActivationComplete() =>
        RefreshNews();
}