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
        set => SetAndNotifyWithConfig(ref _showPreviousRaces, c => c.ShowPreviousRaces, value);
    }

    private bool _enableNotifications;
    public bool EnableNotifications
    {
        get => _enableNotifications;
        set => SetAndNotifyWithConfig(ref _enableNotifications, c => c.EnableNotifications, value);
    }

    private bool _enableThirtyMinuteNotifications;
    public bool EnableThirtyMinuteNotifications
    {
        get => _enableThirtyMinuteNotifications;
        set => SetAndNotifyWithConfig(ref _enableThirtyMinuteNotifications, c => c.EnableThirtyMinuteNotifications,
            value);
    }

    private bool _enableDayNotifications;
    public bool EnableDayNotifications
    {
        get => _enableDayNotifications;
        set => SetAndNotifyWithConfig(ref _enableDayNotifications, c => c.EnableDayNotifications, value);
    }

    private bool _enableWeekNotifications;
    public bool EnableWeekNotifications
    {
        get => _enableWeekNotifications;
        set => SetAndNotifyWithConfig(ref _enableWeekNotifications, c => c.EnableRaceWeekNotifications, value);
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
    private readonly INotificationService _notifications;
    private readonly GlobalConfigService _global;
    private readonly ITimeService _time;

    private static readonly TimeSpan NotificationTime = TimeSpan.FromMinutes(30);

    public CalendarRootViewModel(ErgastAPIService api,
        INotificationService notifications,
        GlobalConfigService global,
        ITimeService time)
        : base("Calendar", PackIconMaterialKind.Calendar, 0)
    {
        _api = api;
        _notifications = notifications;
        _global = global;
        _time = time;
        _time.RegisterTickCallback(Every.OneSecond, UpdateTimers);
        FeatureLoading = true;
    }

    protected override void OnConfigLoaded()
    {
        ShowPreviousRaces = Config.ShowPreviousRaces;
        EnableNotifications = Config.EnableNotifications;
        EnableThirtyMinuteNotifications = Config.EnableThirtyMinuteNotifications;
        EnableDayNotifications = Config.EnableDayNotifications;
        EnableWeekNotifications = Config.EnableRaceWeekNotifications;
    }

    //protected override async void OnFeatureFirstOpened() => await LoadData();

    //protected override async void OnInitialActivate() => await LoadData();

    public override async Task LoadDataInBackground()
    {
        await base.LoadDataInBackground();
        await LoadData();
    }

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
            .Select(x => new RaceViewModel(x, data.result.ScheduleData.Total, _global, _time));

        foreach (var race in races)
        {
            race.OnNextSessionChanged += SetNotifications;
            Races.Add(race);
            await Task.Delay(5);
        }

        FeatureLoading = false;
        UpdateTimers(_time.OffsetNow);
    }

    private void UpdateTimers(DateTimeOffset offsetNow)
    {
        if (Races.Count <= 0) return;
        if (NextRace is null)
            NextRace = Races.GetNextSession(offsetNow);
        else if (offsetNow >= NextRace.SessionTime)
        {
            NextRace.SetWeekendFinished();
            NextRace = Races.GetNextSession(offsetNow);
        }

        NextRace.UpdateNextSession(offsetNow);
        TimeUntilNextRace = NextRace.SessionTime - offsetNow;
        TimeUntilNextSession = NextRace.NextSession.SessionTime - offsetNow;
    }

    public void ToggleShowPreviousRaces() =>
        ShowPreviousRaces = !ShowPreviousRaces;

    private void SetNotifications()
    {
        _notifications.ScheduleNotification(this,
            NextRace.NextSession.SessionTime - NotificationTime,
            NextRace.Name,
            () => $"{NextRace.NextSession.Name} Starts In {(NextRace.NextSession.SessionTime - _time.OffsetNow).Minutes} Minutes",
            shouldShow: _ => EnableNotifications
                             && EnableThirtyMinuteNotifications);

        _notifications.ScheduleNotification(this,
            _time.StartOfDay(NextRace.NextSession.SessionTime),
            NextRace.Name,
            () => $"{NextRace.NextSession.Name} is today at {NextRace.NextSession.SessionTime.LocalDateTime.ToShortString(_global.Use24HourClock)}",
            shouldShow: t => EnableNotifications
                             && EnableDayNotifications
                             && !_time.IsWithinMinutesBefore(t, NextRace.NextSession.SessionTime, 30)
                             && !_time.IsSameDay(NextRace.NextSession.SessionTime, Config.DayNotificationSentFor),
            onShow: t => Config.DayNotificationSentFor = t);

        _notifications.ScheduleNotification(this,
            _time.StartOfWeek(NextRace.NextSession.SessionTime),
            NextRace.Name,
            () => "It's race week!",
            shouldShow: t => EnableNotifications
                             && EnableWeekNotifications
                             && !_time.IsWithinMinutesBefore(t, NextRace.NextSession.SessionTime, 30)
                             && !_time.IsToday(NextRace.NextSession.SessionTime)
                             && !_time.IsSameWeek(NextRace.NextSession.SessionTime, Config.RaceWeekNotificationSentFor),
            onShow: t => Config.RaceWeekNotificationSentFor = t);
    }
}