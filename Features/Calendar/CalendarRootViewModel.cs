using System;
using System.ComponentModel;
using System.Windows.Data;
using F1Desktop.Features.Base;
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
    private ICollectionView _racesView;

    public CalendarRootViewModel(ErgastAPIService api)
    {
        _api = api;
        _racesView = CollectionViewSource.GetDefaultView(Races);
        _racesView.Filter = FilterRaces;
        _racesView.SortDescriptions.Clear();
        _racesView.SortDescriptions.Add(new SortDescription("DateTime", ListSortDirection.Descending));
    }

    protected override async void OnActivate()
    {
        var data = await _api.GetScheduleAsync();
        if (data is null) return;
        foreach (var race in data.ScheduleData.RaceTable.Races)
        {
            Races.Add(new RaceViewModel(race));
        }
        _racesView.Refresh();
    }

    private bool FilterRaces(object obj)
    {
        var race = (RaceViewModel)obj;
        if (ShowPreviousRaces) return true;
        return race.DateTime < DateTimeOffset.Now;
    }
}