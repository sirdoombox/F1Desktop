using System.Windows;

namespace F1Desktop.Misc.Extensions;

public static class UIElementExtensions
{
    public static Window AsWindow(this UIElement uiElement) => (Window)uiElement;
}