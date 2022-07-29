﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using F1Desktop.Features.Base;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.Config;
using F1Desktop.Services;
using FluentScheduler;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class CalendarRootViewModel : FeatureRootBase<CalendarConfig>
{
    public BindableCollection<RaceViewModel> Races { get; } = new();

    private bool _showPreviousRaces;
    public bool ShowPreviousRaces
    {
        get => _showPreviousRaces;
        set
        {
            SetAndNotify(ref _showPreviousRaces, value);
            Config.ShowPreviousRaces = value;
            _racesView.Refresh();
        }
    }

    private TimeSpan _timeUntilNextSession;
    public TimeSpan TimeUntilNextSession
    {
        get => _timeUntilNextSession;
        set => SetAndNotify(ref _timeUntilNextSession, value);
    }
    
    private TimeSpan _timeUntilNextRace;
    public TimeSpan TimeUntilNextRace
    {
        get => _timeUntilNextRace;
        set => SetAndNotify(ref _timeUntilNextRace, value);
    }

    private RaceViewModel _nextRace;
    
    private readonly ErgastAPIService _api;
    private readonly ICollectionView _racesView;

    public CalendarRootViewModel(ErgastAPIService api, ConfigService configService) : base(configService)
    {
        _api = api;
        _racesView = CollectionViewSource.GetDefaultView(Races);
        _racesView.Filter = FilterRaces;
        _racesView.SortDescriptions.Clear();
        _racesView.SortDescriptions.Add(new SortDescription("RaceNumber", ListSortDirection.Ascending));
        JobManager.AddJob(UpdateTimers, s => s.ToRunEvery(10).Seconds());
    }

    private void UpdateTimers()
    {
        if (Races.Count <= 0) return;
        if (_nextRace is null)
            _nextRace = Races.GetNextSession();
        else if (DateTimeOffset.Now >= _nextRace.SessionTime)
        {
            _nextRace.IsNext = false;
            _nextRace = Races.GetNextSession();
        }
        _nextRace.UpdateNextSession();
        TimeUntilNextRace = _nextRace.SessionTime - DateTimeOffset.Now;
        TimeUntilNextSession = _nextRace.NextSession.SessionTime - DateTimeOffset.Now;
    }

    protected override void OnConfigLoaded()
    {
        ShowPreviousRaces = Config.ShowPreviousRaces;
    }

    protected override async void OnActivationComplete()
    {
        Races.Clear();
        var data = await _api.GetScheduleAsync();
        if (data is null) return;
        Races.AddRange(data.ScheduleData.RaceTable.Races.Select(x => new RaceViewModel(x, data.ScheduleData.Total)));
        _nextRace = Races.GetNextSession();
        _racesView.Refresh();
        UpdateTimers();
    }

    private bool FilterRaces(object obj)
    {
        if (ShowPreviousRaces) return true;
        var race = (RaceViewModel)obj;
        return race.SessionTime > DateTimeOffset.Now;
    }
}