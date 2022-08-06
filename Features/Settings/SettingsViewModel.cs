using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
using F1Desktop.Services;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Settings;

public class SettingsViewModel : FeatureBaseWithConfig<GlobalConfig>
{
    public SettingsViewModel(ConfigService configService) 
        : base("Settings", PackIconMaterialKind.Cog, configService, byte.MaxValue)
    {
        
    }
}