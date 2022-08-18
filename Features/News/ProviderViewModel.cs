namespace F1Desktop.Features.News;

public class ProviderViewModel : PropertyChangedBase
{
    public string ProviderName { get; }

    private bool _isEnabled;
    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetAndNotify(ref _isEnabled, value);
    }

    public ProviderViewModel(string providerName, bool isEnabled, Action<string, bool> propChanged)
    {
        ProviderName = providerName;
        IsEnabled = isEnabled;
        PropertyChanged += (_, _) => propChanged?.Invoke(ProviderName, IsEnabled);
    }
}