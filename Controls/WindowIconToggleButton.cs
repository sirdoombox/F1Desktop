using System.Windows;
using System.Windows.Controls.Primitives;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Controls;

public class WindowIconToggleButton : ToggleButton
{
    public static readonly DependencyProperty OnIconKindProperty = DependencyProperty.Register(
        "OnIconKind", typeof(PackIconMaterialKind), typeof(WindowIconToggleButton), new PropertyMetadata(default(PackIconMaterialKind)));

    public PackIconMaterialKind OnIconKind
    {
        get => (PackIconMaterialKind)GetValue(OnIconKindProperty);
        set => SetValue(OnIconKindProperty, value);
    }

    public static readonly DependencyProperty OffIconKindProperty = DependencyProperty.Register(
        "OffIconKind", typeof(PackIconMaterialKind), typeof(WindowIconToggleButton), new PropertyMetadata(default(PackIconMaterialKind)));

    public PackIconMaterialKind OffIconKind
    {
        get => (PackIconMaterialKind)GetValue(OffIconKindProperty);
        set => SetValue(OffIconKindProperty, value);
    }

    public static readonly DependencyProperty OnTooltipProperty = DependencyProperty.Register(
        "OnTooltip", typeof(string), typeof(WindowIconToggleButton), new PropertyMetadata(default(string)));

    public string OnTooltip
    {
        get => (string)GetValue(OnTooltipProperty);
        set => SetValue(OnTooltipProperty, value);
    }

    public static readonly DependencyProperty OffTooltipProperty = DependencyProperty.Register(
        "OffTooltip", typeof(string), typeof(WindowIconToggleButton), new PropertyMetadata(default(string)));

    public string OffTooltip
    {
        get => (string)GetValue(OffTooltipProperty);
        set => SetValue(OffTooltipProperty, value);
    }
}