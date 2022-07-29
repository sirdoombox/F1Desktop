using System.Windows;
using System.Windows.Controls;

namespace F1Desktop.Controls;

public class CountdownElement : Control
{
    public static readonly DependencyProperty CountdownValueProperty = DependencyProperty.Register(
        "CountdownValue", typeof(int), typeof(CountdownElement), new PropertyMetadata(default(int)));

    public int CountdownValue
    {
        get => (int)GetValue(CountdownValueProperty);
        set => SetValue(CountdownValueProperty, value);
    }

    public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register(
        "DisplayText", typeof(string), typeof(CountdownElement), new PropertyMetadata(default(string)));

    public string DisplayText
    {
        get => (string)GetValue(DisplayTextProperty);
        set => SetValue(DisplayTextProperty, value);
    }
}