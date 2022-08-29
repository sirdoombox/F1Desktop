using System.Windows;
using AdonisUI.Controls;

namespace F1Desktop.Controls;

/// <summary>
/// Prevents the window from losing focus or being hidden.
/// </summary>
public class ArmouredAdonisWindow : AdonisWindow
{
    protected override void OnDeactivated(EventArgs e)
    {
        Topmost = true;
        Activate();
    }

    protected override void MinimizeClick(object sender, RoutedEventArgs e)
    {
        e.Handled = true;
    }
}