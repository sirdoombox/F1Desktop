using F1Desktop.Enums;
using F1Desktop.Features.Base;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.Config;
using F1Desktop.Services;
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
        Races.AddRange(data.ScheduleData.RaceTable.Races.OrderBy(x => x.DateTime).Select(x => new RaceViewModel(x, data.ScheduleData.Total)));
        NextRace = Races.GetNextSession();
        UpdateTimers();
    }
}