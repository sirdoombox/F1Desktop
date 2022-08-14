﻿using System.Windows;
using F1Desktop.Models.Config;
using F1Desktop.Services.Local;
using JetBrains.Annotations;
using Stylet;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class RootViewModel : Conductor<IScreen>.Collection.AllActive
{
    private readonly IWindowManager _wm;
    private readonly WindowViewModel _window;
    private readonly ThemeService _theme;
    private readonly GlobalConfig _cfg;

    public RootViewModel(IWindowManager wm, WindowViewModel window, GlobalConfig cfg, ThemeService theme)
    {
        _wm = wm;
        _window = window;
        _theme = theme;
        _cfg = cfg;
    }

    protected override void OnViewLoaded()
    {
        _theme.SetTheme(_cfg.LightTheme);
    }

    public void OpenWindow(Type toOpen)
    {
        _wm.ShowWindow(_window);
        ((Window)_window.View).Activate();
        _window.ActiveViewModel = _window.Features.First(x => x.GetType() == toOpen);
    }

    public void Exit() => Application.Current.Shutdown();
}