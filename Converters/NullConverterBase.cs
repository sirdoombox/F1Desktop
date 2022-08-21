using System.Globalization;
using System.Windows.Data;

namespace F1Desktop.Converters;

public abstract class NullConverterBase<T> : IValueConverter
{
    private readonly T _isNull;
    private readonly T _notNull;
    
    protected NullConverterBase(T isNull, T notNull)
    {
        _isNull = isNull;
        _notNull = notNull;
    }

    public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is null ? _isNull : _notNull;

    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}