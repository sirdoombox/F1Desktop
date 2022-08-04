using System.Text.RegularExpressions;
using System.Web;

namespace F1Desktop.Misc.Extensions;

public static class StringExtensions
{
    private static readonly Regex Tags = new("<.*?>", RegexOptions.Compiled);
    
    public static string HtmlDecode(this string s)
    {
        return HttpUtility.HtmlDecode(Tags.Replace(s, " "));
    }
    
    public static string TruncateForDisplay(this string value, int length = 255)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        var returnValue = value;
        if (value.Length <= length) return returnValue;
        var tmp = value[..length] ;
        if (tmp.LastIndexOf(' ') > 0)
            returnValue = tmp[..tmp.LastIndexOf(' ')] + " [...]";
        return returnValue;
    }
}