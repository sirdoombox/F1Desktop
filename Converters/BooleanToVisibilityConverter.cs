using System.Windows;

namespace F1Desktop.Converters;

public class BooleanToVisibilityConverter : BooleanConverterBase<Visibility>
{
    public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
    {
    }
}