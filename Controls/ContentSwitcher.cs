using System.Windows;
using System.Windows.Controls;

namespace F1Desktop.Controls;

public class ContentSwitcher : Control
{
    public static readonly DependencyProperty UseAlternateContentProperty = DependencyProperty.Register(
        nameof(UseAlternateContent), typeof(bool), typeof(ContentSwitcher), new PropertyMetadata(default(bool)));

    public bool UseAlternateContent
    {
        get => (bool)GetValue(UseAlternateContentProperty);
        set => SetValue(UseAlternateContentProperty, value);
    }

    public static readonly DependencyProperty DefaultContentProperty = DependencyProperty.Register(
        nameof(DefaultContent), typeof(object), typeof(ContentSwitcher), new PropertyMetadata(default(object)));

    public object DefaultContent
    {
        get => (object)GetValue(DefaultContentProperty);
        set => SetValue(DefaultContentProperty, value);
    }

    public static readonly DependencyProperty AlternateContentProperty = DependencyProperty.Register(
        nameof(AlternateContent), typeof(object), typeof(ContentSwitcher), new PropertyMetadata(default(object)));

    public object AlternateContent
    {
        get => (object)GetValue(AlternateContentProperty);
        set => SetValue(AlternateContentProperty, value);
    }
}