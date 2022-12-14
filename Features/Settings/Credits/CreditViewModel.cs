using F1Desktop.Misc;
using F1Desktop.Models.Resources;

namespace F1Desktop.Features.Settings.Credits;

public class CreditViewModel : PropertyChangedBase
{
    public string Name { get; }
    public string Description { get; }
    public string Url { get; }
    public string LicenseUrl { get; }

    public CreditViewModel(CreditData data)
    {
        Name = data.Name;
        Description = data.Description;
        Url = data.Url;
        LicenseUrl = data.LicenseUrl;
    }

    public void OpenUrl() => UrlHelper.Open(Url);
    public void OpenLicense() => UrlHelper.Open(LicenseUrl);
}