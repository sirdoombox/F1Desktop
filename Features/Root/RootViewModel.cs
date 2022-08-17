﻿using System.Windows;
using F1Desktop.Services.Local;
using JetBrains.Annotations;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class RootViewModel : Conductor<IScreen>.Collection.AllActive
{
    private readonly IWindowManager _wm;
    private readonly WindowViewModel _window;
    private readonly GlobalConfigService _cfg;

    public RootViewModel(IWindowManager wm, WindowViewModel window, GlobalConfigService cfg)
    {
        _wm = wm;
        _window = window;
        _cfg = cfg;
    }

    public void OpenWindow(Type toOpen)
    {
        _wm.ShowWindow(_window);
        ((Window)_window.View).Activate();
        _window.OpenFeature(toOpen);
    }

    public void OpenDefault()
    {
        _wm.ShowWindow(_window);
        ((Window)_window.View).Activate();
        _window.OpenFeature(_cfg.LastOpenedFeature);
    }

    public void Exit() => Application.Current.Shutdown();
}