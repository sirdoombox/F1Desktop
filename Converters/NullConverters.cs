using System.Windows;

namespace F1Desktop.Converters;

public class NullToBoolConverter : NullConverterBase<bool>
{
    public NullToBoolConverter() : base(true, false) { }
}

public class InverseNullToBoolConverter : NullConverterBase<bool>
{
    public InverseNullToBoolConverter() : base(false, true) { }
}

public class NullToVisibilityConverter : NullConverterBase<Visibility>
{
    public NullToVisibilityConverter() : base(Visibility.Collapsed, Visibility.Visible) { }
}

public class InverseNullToVisibilityConverter : NullConverterBase<Visibility>
{
    public InverseNullToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { }
}