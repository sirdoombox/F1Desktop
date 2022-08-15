using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace F1Desktop.Controls;

// Prevents the popup being closed when you click anywhere in it's bounds - kinda ridiculous that this is necessary.
public class ArmouredPopup : Popup
{
    protected override void OnMouseDown(MouseButtonEventArgs e) => e.Handled = true;
}