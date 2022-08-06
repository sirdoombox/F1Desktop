using F1Desktop.Attributes;

namespace F1Desktop.Models.Resources;

[Filename("Credits.json")]
public class CreditData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string LicenseUrl { get; set; }
}