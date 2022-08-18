using System.ComponentModel;
using System.Windows.Data;
using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
using F1Desktop.Services.Interfaces;
using F1Desktop.Services.Local;
using F1Desktop.Services.Remote;
using JetBrains.Annotations;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.News;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class NewsRootViewModel : FeatureBaseWithConfig<NewsConfig>
{
    public List<int> DayIncrements { get; } = new() { 1, 7, 14, 30, 365 };
    public List<int> ArticleIncrements { get; } = new() { 10, 25, 50, 100 };

    public BindableCollection<NewsItemViewModel> NewsItems { get; } = new();
    public BindableCollection<ProviderViewModel> Providers { get; } = new();

    private bool _isNewsItemsUnavailable;

    public bool IsNewsItemsUnavailable
    {
        get => _isNewsItemsUnavailable;
        set => SetAndNotify(ref _isNewsItemsUnavailable, value);
    }

    private int _maxArticles;

    public int MaxArticles
    {
        get => _maxArticles;
        set
        {
            if (!SetAndNotifyWithConfig(ref _maxArticles, c => c.MaxArticles, value)) return;
            _newsItemsFilter.Refresh();
        }
    }

    private DateTimeOffset _articleCutoff;
    private int _maxDays;

    public int MaxDays
    {
        get => _maxDays;
        set
        {
            if (!SetAndNotifyWithConfig(ref _maxDays, c => c.MaxDays, value)) return;
            _articleCutoff = DateTimeOffset.Now - TimeSpan.FromDays(_maxDays);
            _newsItemsFilter.Refresh();
        }
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
            var index = NewsItems.IndexOf(newsItem);
            if (index > MaxArticles || newsItem.Published < _articleCutoff) return false;
            return Providers.First(x => x.ProviderName == newsItem.ProviderName).IsEnabled;
        };
        NewsItems.CollectionChanged += (_, _) => IsNewsItemsUnavailable = NewsItems.Count <= 0;
    }

    protected override void OnConfigLoaded()
    {
        MaxArticles = Config.MaxArticles;
        MaxDays = Config.MaxDays;
    }

    protected override void OnFeatureFirstOpened()
    {
        var providers = _rss.GetProviders();
        foreach (var provider in providers)
        {
            var isEnabled = true;
            if (!Config.Providers.ContainsKey(provider))
                Config.Providers.Add(provider, true);
            else
                isEnabled = Config.Providers[provider];
            Providers.Add(new ProviderViewModel(provider, isEnabled, ProviderStatusChanged));
        }

        RefreshNews();
    }

    private void ProviderStatusChanged(string provider, bool isEnabled)
    {
        _newsItemsFilter.Refresh();
        Config.Providers[provider] = isEnabled;
    }

    public async void RefreshNews()
    {
        NewsItems.Clear();
        var items = await _rss.GetNewsAsync();
        var newsItems = items.Select(newsItem => new NewsItemViewModel(newsItem, _global))
            .OrderByDescending(x => x.Published);
        NewsItems.AddRange(newsItems);
    }
}