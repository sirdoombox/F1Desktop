using System.Globalization;
using System.Windows.Data;

namespace F1Desktop.Converters;

public class BooleanToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool b) return string.Empty;
        if (parameter is not string s) return string.Empty;
        var parameters = s.Split('|');
        if (parameters.Length != 2) return s;
        return b ? parameters[0] : parameters[1];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}