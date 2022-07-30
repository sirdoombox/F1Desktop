using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace F1Desktop.Controls;

[TemplatePart(Name = "PART_TextBox", Type = typeof(TextBlock))]
public class HighlightableTextBlock : Control
{
    public static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register(
        "IsHighlighted", typeof(bool), typeof(HighlightableTextBlock), new PropertyMetadata(default(bool)));
    public bool IsHighlighted
    {
        get => (bool)GetValue(IsHighlightedProperty);
        set => SetValue(IsHighlightedProperty, value);
    }

    public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.Register(
        "HighlightBrush", typeof(Brush), typeof(HighlightableTextBlock), new PropertyMetadata(default(Brush)));
    public Brush HighlightBrush
    {
        get => (Brush)GetValue(HighlightBrushProperty);
        set => SetValue(HighlightBrushProperty, value);
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        "Text", typeof(string), typeof(HighlightableTextBlock), new PropertyMetadata(default(string)));
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
        "TextAlignment", typeof(TextAlignment), typeof(HighlightableTextBlock), new PropertyMetadata(default(TextAlignment)));
    public TextAlignment TextAlignment
    {
        get => (TextAlignment)GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }

    static HighlightableTextBlock()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HighlightableTextBlock), new FrameworkPropertyMetadata(typeof(HighlightableTextBlock)));
    }
}