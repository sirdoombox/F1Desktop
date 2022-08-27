using System.Threading.Tasks;
using F1Desktop.Enums;
using F1Desktop.Features.Base;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.Config;
using F1Desktop.Models.ErgastAPI.Schedule;
using F1Desktop.Services.Interfaces;
using F1Desktop.Services.Local;
using F1Desktop.Services.Remote;
using JetBrains.Annotations;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Calendar;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class CalendarRootViewModel : FeatureBaseWithConfig<CalendarConfig>
{
    public BindableCollection<RaceViewModel> Races { get; } = new();

    private bool _showPreviousRaces;
    public bool ShowPreviousRaces
    {
        get => _showPreviousRaces;
        private set => SetAndNotifyWithConfig(ref _showPreviousRaces, x => x.ShowPreviousRaces, value);
    }

    private bool _enableNotifications;
    public bool EnableNotifications
    {
        get => _enableNotifications;
        private set => SetAndNotifyWithConfig(ref _enableNotifications, x => x.EnableNotifications, value);
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
    private readonly GlobalConfigService _global;

    private static readonly TimeSpan NotificationTime = TimeSpan.FromMinutes(30);

    public CalendarRootViewModel(ErgastAPIService api,
        NotificationService notifications,
        GlobalConfigService global,
        TickService tick)
        : base("Calendar", PackIconMaterialKind.Calendar, 0)
    {
        _api = api;
        _notifications = notifications;
        _global = global;
        tick.TenSeconds += UpdateTimers;
        FeatureLoading = true;
    }

    protected override void OnConfigLoaded()
    {
        ShowPreviousRaces = Config.ShowPreviousRaces;
        EnableNotifications = Config.EnableNotifications;
    }

    protected override async void OnFeatureFirstOpened() => await LoadData();

    public Task RefreshData() => LoadData();
    
    private async Task LoadData()
    {
        FeatureLoading = true;
        NextRace = null;
        Races.Clear();
        var data = await _api.GetAsync<ScheduleRoot>();
        if (data.status != ApiRequestStatus.Success) return;
        var races = data.result.ScheduleData.RaceTable.Races
            .OrderBy(x => x.DateTime)
            .Select(x => new RaceViewModel(x, data.result.ScheduleData.Total, _global));
        foreach (var race in races)
        {
            Races.Add(race);
            await Task.Delay(5);
        }
        FeatureLoading = false;
        UpdateTimers();
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

        hasNextSessionChanged = NextRace.UpdateNextSession();

        TimeUntilNextRace = NextRace.SessionTime - DateTimeOffset.Now;
        TimeUntilNextSession = NextRace.NextSession.SessionTime - DateTimeOffset.Now;
        SetNotifications(hasNextRaceChanged, hasNextSessionChanged);
    }

    public void ToggleShowPreviousRaces() =>
        ShowPreviousRaces = !ShowPreviousRaces;

    public void ToggleEnableNotifications() => 
        EnableNotifications = !EnableNotifications;

    private void SetNotifications(bool raceChanged, bool sessionChanged)
    {
        if (raceChanged)
        {
            _notifications.ScheduleNotification(this,
                NextRace.SessionTime - NotificationTime,
                NextRace.Name,
                () => $"Lights Out In {(NextRace.SessionTime - DateTimeOffset.Now).Minutes} Minutes",
                shouldShow: () => EnableNotifications);
        }

        if (sessionChanged && NextRace.NextSession.Type != SessionType.Race)
        {
            _notifications.ScheduleNotification(this,
                NextRace.NextSession.SessionTime - NotificationTime,
                NextRace.Name,
                () => $"{NextRace.NextSession.Name} Starts In {(NextRace.NextSession.SessionTime - DateTimeOffset.Now).Minutes} Minutes.",
                shouldShow: () => EnableNotifications);
        }
    }
}