using System.Windows;
using System.Windows.Controls;

namespace F1Desktop.Helpers;

public class MultiContentHelper : DependencyObject
{
    public static readonly DependencyProperty UseAlternateContentProperty = DependencyProperty.RegisterAttached(
        "UseAlternateContent", typeof(bool), typeof(MultiContentHelper), new PropertyMetadata(default(bool), OnAnyChanged));
    
    public static bool GetUseAlternateContent(DependencyObject obj) =>
        (bool)obj.GetValue(UseAlternateContentProperty);

    public static void SetUseAlternateContent(DependencyObject obj, object newValue) =>
        obj.SetValue(UseAlternateContentProperty, newValue);

    public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(
        "Content", typeof(object), typeof(MultiContentHelper), new PropertyMetadata(default(object), OnAnyChanged));

    public static object GetContent(DependencyObject obj) =>
        (object)obj.GetValue(ContentProperty);

    public static void SetContent(DependencyObject obj, object newValue) =>
        obj.SetValue(ContentProperty, newValue);

    public static readonly DependencyProperty AlternateContentProperty = DependencyProperty.RegisterAttached(
        "AlternateContent", typeof(object), typeof(MultiContentHelper), new PropertyMetadata(default(object), OnAnyChanged));

    public static object GetAlternateContent(DependencyObject obj) =>
        (object)obj.GetValue(AlternateContentProperty);

    public static void SetAlternateContent(DependencyObject obj, object newValue) =>
        obj.SetValue(AlternateContentProperty, newValue);

    public static readonly DependencyProperty CurrentContentProperty = DependencyProperty.RegisterAttached(
        "CurrentContent", typeof(object), typeof(MultiContentHelper), new PropertyMetadata(default(object)));

    public static object GetCurrentContent(DependencyObject obj) =>
        (object)obj.GetValue(CurrentContentProperty);

    public static void SetCurrentContent(DependencyObject obj, object newValue) =>
        obj.SetValue(CurrentContentProperty, newValue);
    
    private static void OnAnyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var useAlternate = GetUseAlternateContent(d);
        var content = GetContent(d);
        var altContent = GetAlternateContent(d);
        var toUse = useAlternate ? altContent : content;

        if (d is ContentControl cc)
            cc.Content = toUse;
        else
            SetCurrentContent(d, toUse);
    }
}