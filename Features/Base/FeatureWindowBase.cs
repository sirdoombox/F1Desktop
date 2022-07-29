using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using JetBrains.Annotations;
using Stylet;

namespace F1Desktop.Features.Base;

public class FeatureWindowBase : Screen
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetForegroundWindow(IntPtr hWnd);
    
    protected bool CloseWindowOnFocusLost { get; set; }

    [UsedImplicitly]
    public void FocusLost(object sender, EventArgs e)
    {
        if (!CloseWindowOnFocusLost || sender is not Window win) return;
        try
        {
            win.Close();
        }
        catch
        {
            // ignored
        }
    }
    
    protected override void OnActivate()
    {
        if (View is not Window win) return;
        SetForegroundWindow(new WindowInteropHelper(win).EnsureHandle());
    }
}