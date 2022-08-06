using F1Desktop.Attributes;

namespace F1Desktop.Models.Resources;

[Filename("Countries.json")]
public class CountryData
{
    public string IsoCode { get; set; }
    public string Name { get; set; }
    public List<string> Adjectives { get; set; }
}