using System.ComponentModel;
using System.Windows.Data;
using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
using F1Desktop.Services.Interfaces;
using F1Desktop.Services.Local;
using F1Desktop.Services.Remote;
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
    private readonly GlobalConfigService _global;
    private readonly ICollectionView _newsItemsFilter;

    public NewsRootViewModel(IConfigService cfg, NewsRssService rss, GlobalConfigService global)
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
        NewsItems.CollectionChanged += (_, _) => IsNewsItemsUnavailable = NewsItems.Count <= 0;
        foreach (var provider in Providers)
            provider.PropertyChanged += (_, _) => _newsItemsFilter.Refresh();
    }

    protected override void OnFeatureFirstOpened()
    {
        Providers.AddRange(_rss.GetProviders().Select(x => new ProviderViewModel(x, true)));
        RefreshNews();
    }

    public async void RefreshNews()
    {
        NewsItems.Clear();
        var items = await _rss.GetNewsAsync();
        var newsItems = items.Select(newsItem => new NewsItemViewModel(newsItem, _global)
            {
                Use24HourClock = _global.Use24HourClock
            })
            .OrderByDescending(x => x.Published);
        NewsItems.AddRange(newsItems);
    }
}