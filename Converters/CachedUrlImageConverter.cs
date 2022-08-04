using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace F1Desktop.Converters;

public class CachedUrlImageConverter : IValueConverter
{
    private static readonly Dictionary<string, BitmapImage> Cache = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string s)
        {
            if (Cache.TryGetValue(s, out var i)) return i;
            var img = new BitmapImage(new Uri(s, UriKind.Absolute));
            Cache.Add(s, img);
            return img;
        } 
        if (value is Uri u)
        {
            if (Cache.TryGetValue(u.AbsoluteUri, out var i)) return i;
            var img = new BitmapImage(u);
            Cache.Add(u.AbsoluteUri,img);
            return img;
        }

        throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}