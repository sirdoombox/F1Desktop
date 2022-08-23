using System.Globalization;
using System.Windows.Data;

namespace F1Desktop.Converters;

public class IntThresholdToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not int i) return false;
        var threshold = parameter is not int param ? 0 : param;
        return i > threshold;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}