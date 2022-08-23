using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace F1Desktop.Converters;

public class IntThresholdToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not int i) return Visibility.Collapsed;
        var threshold = parameter is not int param ? 0 : param;
        return i > threshold ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}