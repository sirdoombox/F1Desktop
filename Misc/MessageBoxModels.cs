using AdonisUI.Controls;

namespace F1Desktop.Misc;

public static class MessageBoxModels
{
    public static readonly MessageBoxModel ResetToDefault = new()
    {
        Buttons = MessageBoxButtons.YesNo(),
        Icon = MessageBoxImage.Warning,
        Caption = "Reset Settings To Default.",
        Text = "Are you sure you want to reset settings back to the default?"
    };

    public static readonly MessageBoxModel ChangeLog = new()
    {
        Buttons = new [] {MessageBoxButtons.Ok()}
    };
}