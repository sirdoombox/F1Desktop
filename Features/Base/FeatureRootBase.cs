using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using JetBrains.Annotations;
using Stylet;

namespace F1Desktop.Features.Base;

public abstract class FeatureRootBase : Screen
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

    [UsedImplicitly]
    public void FocusLost(object sender, EventArgs e)
    {
        if (sender is not Window win) return;
        win.Close();
    }

    protected override void OnActivate()
    {
        if (View is not Window win) return;
        SetForegroundWindow(new WindowInteropHelper(win).EnsureHandle());
    }
}