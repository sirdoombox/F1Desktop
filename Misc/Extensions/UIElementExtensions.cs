using System.Windows;

namespace F1Desktop.Misc.Extensions;

public static class UIElementExtensions
{
    public static Window AsWindow(this UIElement uiElement) => (Window)uiElement;
    public static Window AsWindow(this object objUiElement) => (Window)objUiElement;
}