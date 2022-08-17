using System.Globalization;
using F1Desktop.Features.Calendar;
using F1Desktop.Models.Resources;

namespace F1Desktop.Misc.Extensions;

public static class EnumerableExtensions
{
    public static T GetNextSession<T>(this IEnumerable<T> collection) where T : SessionViewModelBase
    {
        var next = collection.First(x => x.SessionTime > DateTimeOffset.Now);
        next.IsNext = true;
        return next;
    }

    public static string GetCountryCodeForNationality(this IEnumerable<CountryData> data, string nationality)
    {
        return data.FirstOrDefault(x => x.Adjectives.Any(adj => 
            string.Compare(adj,nationality, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0))
            ?.IsoCode;
    }
}