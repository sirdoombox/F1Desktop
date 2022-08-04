using System.Windows;
using System.Windows.Controls.Primitives;

namespace F1Desktop.Controls;

public class WindowIconToggleButton : ToggleButton
{
    public static readonly DependencyProperty OnContentProperty = DependencyProperty.Register(
        nameof(OnContent), typeof(object), typeof(WindowIconToggleButton), new PropertyMetadata(default(object)));

    public object OnContent
    {
        get => (object)GetValue(OnContentProperty);
        set => SetValue(OnContentProperty, value);
    }

    public static readonly DependencyProperty OffContentProperty = DependencyProperty.Register(
        nameof(OffContent), typeof(object), typeof(WindowIconToggleButton), new PropertyMetadata(default(object)));

    public object OffContent
    {
        get => (object)GetValue(OffContentProperty);
        set => SetValue(OffContentProperty, value);
    }

    public static readonly DependencyProperty OnTooltipProperty = DependencyProperty.Register(
        nameof(OnTooltip), typeof(string), typeof(WindowIconToggleButton), new PropertyMetadata(default(string)));

    public string OnTooltip
    {
        get => (string)GetValue(OnTooltipProperty);
        set => SetValue(OnTooltipProperty, value);
    }

    public static readonly DependencyProperty OffTooltipProperty = DependencyProperty.Register(
        nameof(OffTooltip), typeof(string), typeof(WindowIconToggleButton), new PropertyMetadata(default(string)));

    public string OffTooltip
    {
        get => (string)GetValue(OffTooltipProperty);
        set => SetValue(OffTooltipProperty, value);
    }
}