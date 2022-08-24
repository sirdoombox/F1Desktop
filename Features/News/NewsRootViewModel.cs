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

    private List<NewsItemViewModel> _allNewsItems = new();
    public BindableCollection<NewsItemViewModel> NewsItems { get; } = new();
    public BindableCollection<ProviderViewModel> Providers { get; } = new();

    private int _maxArticles;
    public int MaxArticles
    {
        get => _maxArticles;
        set
        {
            if (!SetAndNotifyWithConfig(ref _maxArticles, c => c.MaxArticles, value)) return;
            RefreshFilter();
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
            RefreshFilter();
        }
    }

    private readonly NewsRssService _rss;
    private readonly GlobalConfigService _global;

    public NewsRootViewModel(NewsRssService rss, GlobalConfigService global)
        : base("News", PackIconMaterialKind.Newspaper, 3)
    {
        _rss = rss;
        _global = global;
    }

    protected override void OnConfigLoaded()
    {
        MaxArticles = Config.MaxArticles;
        MaxDays = Config.MaxDays;
    }

    protected override void OnGlobalConfigReset()
    {
        foreach (var provider in Providers)
            provider.IsEnabled = true;
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
        RefreshFilter();
        Config.Providers[provider] = isEnabled;
    }

    public void RefreshFilter()
    {
        NewsItems.Clear();
        NewsItems.AddRange(_allNewsItems
            .Where(x => x.Published >= _articleCutoff)
            .Where(x => Providers.First(p => p.ProviderName == x.ProviderName).IsEnabled)
            .OrderByDescending(x => x.Published)
            .Take(MaxArticles));
    }

    public async void RefreshNews()
    {
        FeatureLoading = true;
        _allNewsItems.Clear();
        NewsItems.Clear();
        var items = await _rss.GetNewsAsync();
        _allNewsItems.AddRange(items.Select(newsItem => new NewsItemViewModel(newsItem, _global)));
        RefreshFilter();
        FeatureLoading = false;
    }
}