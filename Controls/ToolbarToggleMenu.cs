using System.Windows;
using System.Windows.Controls.Primitives;

namespace F1Desktop.Controls;

public class ToolbarToggleMenu : ToggleButton
{
    public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register(
        nameof(ButtonText), typeof(string), typeof(ToolbarToggleMenu), new PropertyMetadata(default(string)));

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }
}