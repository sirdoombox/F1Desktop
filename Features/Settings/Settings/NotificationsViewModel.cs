using F1Desktop.Features.Settings.Settings.Base;
using F1Desktop.Services.Local;

namespace F1Desktop.Features.Settings.Settings;

public class NotificationsViewModel : SetingsCategoryViewModelBase
{
    private bool _enableNotifications;
    public bool EnableNotifications
    {
        get => _enableNotifications;
        set => SetAndNotifyWithConfig(ref _enableNotifications, c => c.EnableNotifications, value);
    }
    
    private bool _enableSound;
    public bool EnableSound
    {
        get => _enableSound;
        set => SetAndNotifyWithConfig(ref _enableSound, c => c.EnableNotificationsSound, value);
    }
    
    public NotificationsViewModel(GlobalConfigService config) : base("Notifications", config)
    {
    }

    protected override void OnGlobalConfigPropertyChanged(string propName)
    {
        EnableNotifications = Config.EnableNotifications;
        EnableSound = Config.EnableNotificationsSound;
    }
}