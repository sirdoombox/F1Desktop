using System.Windows;

namespace F1Desktop.Converters;

public class BooleanInverseConverter : BooleanConverterBase<bool>
{
    public BooleanInverseConverter() : base(false, true) { }
}

public class BooleanToVisibilityConverter : BooleanConverterBase<Visibility>
{
    public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { }
}

public class InverseBooleanToVisibilityConverter : BooleanConverterBase<Visibility>
{
    public InverseBooleanToVisibilityConverter() : base(Visibility.Collapsed, Visibility.Visible) { }
}