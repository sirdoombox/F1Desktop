using System.Globalization;
using System.Windows.Data;

namespace F1Desktop.Converters;

public class NullToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is null;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => 
        throw new NotImplementedException();
}