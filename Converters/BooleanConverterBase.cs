using System.Globalization;
using System.Windows.Data;

namespace F1Desktop.Converters;

public abstract class BooleanConverterBase<T> : IValueConverter
{
    private readonly T _true;
    private readonly T _false;

    protected BooleanConverterBase(T trueValue, T falseValue)
    {
        _true = trueValue;
        _false = falseValue;
    }

    public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is true ? _true : _false;

    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is T val && EqualityComparer<T>.Default.Equals(val, _true);
}