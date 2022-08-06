using System.Windows;

namespace F1Desktop.Converters;

public class InverseBooleanToVisibilityConverter : BooleanConverterBase<Visibility>
{
    public InverseBooleanToVisibilityConverter() : base(Visibility.Collapsed, Visibility.Visible)
    {
    }
}