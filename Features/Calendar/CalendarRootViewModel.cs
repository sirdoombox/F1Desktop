using System;
using System.ComponentModel;
using System.Windows.Data;
using F1Desktop.Features.Base;
using F1Desktop.Models.ErgastAPI.Schedule;
using F1Desktop.Services;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class CalendarRootViewModel : FeatureRootBase
{
    public BindableCollection<RaceViewModel> Races { get; } = new();

    private bool _showPreviousRaces;
    public bool ShowPreviousRaces
    {
        get => _showPreviousRaces;
        set => SetAndNotify(ref _showPreviousRaces, value);
    }
    
    private readonly ErgastAPIService _api;
    private readonly ICollectionView _racesView;

    public CalendarRootViewModel(ErgastAPIService api)
    {
        _api = api;
        _racesView = CollectionViewSource.GetDefaultView(Races);
        _racesView.Filter = FilterRaces;
        _racesView.SortDescriptions.Clear();
        _racesView.SortDescriptions.Add(new SortDescription("DateTime", ListSortDirection.Ascending));
    }

    protected override async void OnInitialActivate()
    {
        Races.Clear();
        var data = await _api.GetScheduleAsync();
        if (data is null) return;
        Race prev = null;
        foreach (var race in data.ScheduleData.RaceTable.Races)
        {
            var isNextRace = (prev is not null 
                             && prev.DateTime < DateTimeOffset.Now 
                             && race.DateTime > DateTimeOffset.Now)
                             || (prev is null
                             && race.DateTime > DateTimeOffset.Now);
            Races.Add(new RaceViewModel(race, data.ScheduleData.Total, isNextRace));
            prev = race;
        }
        ShowPreviousRaces = false;
    }

    private bool FilterRaces(object obj)
    {
        if (ShowPreviousRaces) return true;
        var race = (RaceViewModel)obj;
        return race.DateTime > DateTimeOffset.Now;
    }

    protected override bool SetAndNotify<T>(ref T field, T value, string propertyName = "")
    { 
        var res = base.SetAndNotify(ref field, value, propertyName);
        _racesView?.Refresh();
        return res;
    } 
}