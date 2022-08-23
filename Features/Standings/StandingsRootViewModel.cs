using System.Threading.Tasks;
using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
using F1Desktop.Models.ErgastAPI.ConstructorStandings;
using F1Desktop.Models.ErgastAPI.DriverStandings;
using F1Desktop.Models.ErgastAPI.Schedule;
using F1Desktop.Models.Resources;
using F1Desktop.Services.Interfaces;
using F1Desktop.Services.Local;
using F1Desktop.Services.Remote;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Standings;

public class StandingsRootViewModel : FeatureBaseWIthConfigAndCache<StandingsConfig>
{
    public StandingsTableViewModel DriverStandings { get; }
    public StandingsTableViewModel ConstructorStandings { get; }
    
    private bool _pointsDiffFromLeader;
    public bool PointsDiffFromLeader
    {
        get => _pointsDiffFromLeader;
        set
        {
            SetAndNotifyWithConfig(ref _pointsDiffFromLeader, c => c.PointsDiffFromLeader, value);
            DriverStandings.PointsDiffFromLeader = value;
            ConstructorStandings.PointsDiffFromLeader = value;
        }
    }

    private readonly ErgastAPIService _api;
    private readonly DataResourceService _data;

    public StandingsRootViewModel(IConfigService configService,
        ErgastAPIService api,
        Func<StandingsTableViewModel> standingsTable,
        DataResourceService dataResourceService)
        : base("Standings", PackIconMaterialKind.PodiumGold, configService, 2)
    {
        _api = api;
        _data = dataResourceService;
        DriverStandings = standingsTable();
        ConstructorStandings = standingsTable();
        FeatureLoading = true;
    }

    protected override void OnConfigLoaded()
    {
        PointsDiffFromLeader = Config.PointsDiffFromLeader;
    }

    protected override async void OnFeatureFirstOpened() => 
        await LoadData(false);

    public override async void ForceRefresh() => 
        await LoadData(true);

    private async Task LoadData(bool invalidate)
    {
        // Can't run tasks concurrently right now, they both require the same data to generate the cache invalid time
        // Consider an in-memory cache for these and pre-cache the schedule root ahead of time.
        // var cTask = _api.GetAsync<ConstructorStandingsRoot, ScheduleRoot>((s,_) => GetCacheInvalidTime(s));
        // var dTask = _api.GetAsync<DriverStandingsRoot, ScheduleRoot>((s,_) => GetCacheInvalidTime(s));
        // await Task.WhenAll(cTask, dTask);
        // var countries = await _data.LoadJsonResourceAsync<List<CountryData>>();
        // DriverStandings.InitStandings(
        //     dTask.Result.Data.StandingsTable.StandingsLists[0].DriverStandings, countries);
        // ConstructorStandings.InitStandings(
        //     cTask.Result.Data.StandingsTable.StandingsLists[0].ConstructorStandings, countries);
        
        FeatureLoading = true;
        var constructors = await _api.GetAsync<ConstructorStandingsRoot, ScheduleRoot>((s,_) => GetCacheInvalidTime(s), invalidate);
        var drivers = await _api.GetAsync<DriverStandingsRoot, ScheduleRoot>((s,_) => GetCacheInvalidTime(s), invalidate);
        var countries = await _data.LoadJsonResourceAsync<List<CountryData>>();
        DriverStandings.InitStandings(
            drivers.Data.StandingsTable.StandingsLists[0].DriverStandings, countries);
        ConstructorStandings.InitStandings(
            constructors.Data.StandingsTable.StandingsLists[0].ConstructorStandings, countries);
        CachedAt = drivers.CacheTime;
        CacheInvalidAt = drivers.CacheInvalidAt;
        FeatureLoading = false;
    }

    private static DateTimeOffset GetCacheInvalidTime(ScheduleRoot schedule)
    {
        return schedule.ScheduleData.RaceTable.Races
            .OrderBy(x => x.DateTime)
            .First(x => x.IsUpcoming).DateTime + TimeSpan.FromHours(8);
    }

    public void TogglePointsDiffFromLeader() => 
        PointsDiffFromLeader = !PointsDiffFromLeader;
}