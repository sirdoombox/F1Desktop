using System.Windows;

namespace F1Desktop.Helpers;

public class StringFormatHelper : DependencyObject
{
    public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
        "Value", typeof(object), typeof(StringFormatHelper), new PropertyMetadata(null, OnAnyChanged));

    public static object GetValue(DependencyObject obj) =>
        obj.GetValue(ValueProperty);

    public static void SetValue(DependencyObject obj, object newValue) =>
        obj.SetValue(ValueProperty, newValue);

    public static readonly DependencyProperty UseAlternateFormatProperty = DependencyProperty.RegisterAttached(
        "UseAlternateFormat", typeof(bool), typeof(StringFormatHelper),
        new PropertyMetadata(default(bool), OnAnyChanged));

    public static bool GetUseAlternateFormat(DependencyObject obj) =>
        (bool)obj.GetValue(UseAlternateFormatProperty);

    public static void SetUseAlternateFormat(DependencyObject obj, object newValue) =>
        obj.SetValue(UseAlternateFormatProperty, newValue);

    public static readonly DependencyProperty FormatProperty = DependencyProperty.RegisterAttached(
        "Format", typeof(string), typeof(StringFormatHelper), new PropertyMetadata(null, OnAnyChanged));

    public static string GetFormat(DependencyObject obj) =>
        (string)obj.GetValue(FormatProperty);

    public static void SetFormat(DependencyObject obj, string newFormat) =>
        obj.SetValue(FormatProperty, newFormat);

    public static readonly DependencyProperty AlternateFormatProperty = DependencyProperty.RegisterAttached(
        "AlternateFormat", typeof(string), typeof(StringFormatHelper),
        new PropertyMetadata(default(string), OnAnyChanged));

    public static string GetAlternateFormat(DependencyObject obj) =>
        (string)obj.GetValue(AlternateFormatProperty);

    public static void SetAlternateFormat(DependencyObject obj, object newValue) =>
        obj.SetValue(AlternateFormatProperty, newValue);

    public static readonly DependencyProperty FormattedValueProperty = DependencyProperty.RegisterAttached(
        "FormattedValue", typeof(string), typeof(StringFormatHelper), new PropertyMetadata(null));

    public static string GetFormattedValue(DependencyObject obj) =>
        (string)obj.GetValue(FormattedValueProperty);

    public static void SetFormattedValue(DependencyObject obj, string newFormattedValue) =>
        obj.SetValue(FormattedValueProperty, newFormattedValue);

    private static void OnAnyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) =>
        RefreshFormattedValue(obj);

    private static void RefreshFormattedValue(DependencyObject obj)
    {
        var value = GetValue(obj);
        var useAlternate = GetUseAlternateFormat(obj);
        var format = useAlternate ? GetAlternateFormat(obj) : GetFormat(obj);

        if (format != null)
        {
            if (!format.StartsWith("{0:")) format = $"{{0:{format}}}";
            SetFormattedValue(obj, string.Format(format, value));
        }
        else
            SetFormattedValue(obj, value == null ? string.Empty : value.ToString());
    }
}