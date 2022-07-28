using System;
using System.Windows;
using F1Desktop.Features.Base;
using F1Desktop.Services;
using JetBrains.Annotations;

namespace F1Desktop.Features.Calendar;

public class CalendarRootViewModel : FeatureRootBase
{
    private ErgastAPIService _api;
    
    public CalendarRootViewModel(ErgastAPIService api)
    {
        _api = api;
    }

    protected override async void OnActivate()
    {
        var data = await _api.GetScheduleAsync();
    }
}