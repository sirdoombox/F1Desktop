using F1Desktop.Models.Base;
using F1Desktop.Services.Interfaces;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Base;

public abstract class FeatureBaseWIthConfigAndCache<TConfig> : FeatureBaseWithConfig<TConfig> where TConfig : ConfigBase, new()
{
    private DateTimeOffset _cachedAt;
    public DateTimeOffset CachedAt
    {
        get => _cachedAt;
        set => SetAndNotify(ref _cachedAt, value);
    }
    
    private DateTimeOffset _cacheInvalidAt;
    public DateTimeOffset CacheInvalidAt
    {
        get => _cacheInvalidAt;
        set => SetAndNotify(ref _cacheInvalidAt, value);
    }
    
    public FeatureBaseWIthConfigAndCache(string displayName, PackIconMaterialKind icon, IConfigService cfg, byte order = byte.MinValue) 
        : base(displayName, icon, cfg, order)
    {
    }
    
    public abstract void ForceRefresh();
}