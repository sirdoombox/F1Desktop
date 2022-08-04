using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using F1Desktop.Enums;
using F1Desktop.Features.Base;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.Config;
using F1Desktop.Models.ErgastAPI.Schedule;
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
    public RaceViewModel NextRace
    {
        get => _nextRace;
        private set => SetAndNotify(ref _nextRace, value);
    }
    
    private readonly ErgastAPIService _api;
    private readonly ICollectionView _racesView;
    private readonly NotificationService _notifications;
    
    private static readonly TimeSpan NotificationTime = TimeSpan.FromMinutes(30);

    public CalendarRootViewModel(ErgastAPIService api, 
        NotificationService notifications, 
        ConfigService configService, 
        TickService tick) 
        : base(configService)
    {
        _api = api;
        _notifications = notifications;
        tick.TenSeconds += UpdateTimers;
        _racesView = CollectionViewSource.GetDefaultView(Races);
        _racesView.Filter = FilterRaces;
        _racesView.SortDescriptions.Clear();
        _racesView.SortDescriptions.Add(new SortDescription("RaceNumber", ListSortDirection.Ascending));
        JobManager.AddJob(UpdateTimers, s => s.ToRunEvery(10).Seconds());
    }

    private void UpdateTimers()
    {
        if (Races.Count <= 0) return;
        bool hasNextRaceChanged = false, hasNextSessionChanged = false;
        if (NextRace is null)
        {
            NextRace = Races.GetNextSession();
            hasNextRaceChanged = true;
        }
        else if (DateTimeOffset.Now >= NextRace.SessionTime)
        {
            NextRace.IsNext = false;
            NextRace = Races.GetNextSession();
            hasNextRaceChanged = true;
        }

        if (NextRace.UpdateNextSession()) 
            hasNextSessionChanged = true;
        
        TimeUntilNextRace = NextRace.SessionTime - DateTimeOffset.Now;
        TimeUntilNextSession = NextRace.NextSession.SessionTime - DateTimeOffset.Now;

        if (hasNextRaceChanged)
        {
            _notifications.ScheduleNotification(NextRace.SessionTime - NotificationTime, 
                NextRace.Name, 
                "Lights Out In 30 Minutes.");
        }
        if (hasNextSessionChanged && NextRace.NextSession.Type != SessionType.Race)
        {
            _notifications.ScheduleNotification(NextRace.NextSession.SessionTime - NotificationTime, 
                NextRace.Name, 
                $"{NextRace.NextSession.Name} Starts In 30 Minutes.");
        }
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
        NextRace = Races.GetNextSession();
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