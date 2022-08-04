using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace F1Desktop.Converters;

public class MultiBoolToVisibilityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        foreach (var value in values)
            if (value is true) return Visibility.Visible;
        return Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => 
        throw new NotImplementedException();
}