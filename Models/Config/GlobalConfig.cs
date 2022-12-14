using System.Windows;
using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.Config;

[Filename("Global.cfg")]
public sealed class GlobalConfig : ConfigBase
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double Left { get; set; }
    public double Top { get; set; }
    public WindowState State { get; set; }
    public bool LightTheme { get; set; }
    public bool Use24HourClock { get; set; }
    public Type LastOpenedFeature { get; set; }
    public bool StartWithWindows { get; set; }
    public bool ShowWindowOnStartup { get; set; }
    public bool EnableNotifications { get; set; }
    public bool EnableNotificationsSound { get; set; }

    public GlobalConfig()
    {
        Width = 1100;
        Height = 800;
        Left = SystemParameters.PrimaryScreenWidth / 2 - Width / 2;
        Top = SystemParameters.PrimaryScreenHeight / 2 - Height / 2;
        State = WindowState.Normal;
        LastOpenedFeature = null;
        Default();
    }

    public override void Default()
    {
        LightTheme = false;
        Use24HourClock = true;
        StartWithWindows = true;
        ShowWindowOnStartup = true;
        EnableNotifications = true;
        EnableNotificationsSound = true;
    }
}