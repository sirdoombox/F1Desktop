using System.Threading.Tasks;
using F1Desktop.Enums;
using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
using F1Desktop.Models.ErgastAPI.ConstructorStandings;
using F1Desktop.Models.ErgastAPI.DriverStandings;
using F1Desktop.Models.Resources;
using F1Desktop.Services.Interfaces;
using F1Desktop.Services.Local;
using F1Desktop.Services.Remote;
using MahApps.Metro.IconPacks;

namespace F1Desktop.Features.Standings;

public class StandingsRootViewModel : FeatureBaseWithConfig<StandingsConfig>
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

    public StandingsRootViewModel(ErgastAPIService api,
        Func<StandingsTableViewModel> standingsTable,
        DataResourceService dataResourceService)
        : base("Standings", PackIconMaterialKind.PodiumGold, 2)
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
        await LoadData();

    public Task RefreshData() => LoadData();

    private async Task LoadData()
    {
        FeatureLoading = true;
        var cTask = _api.GetAsync<ConstructorStandingsRoot>();
        var dTask = _api.GetAsync<DriverStandingsRoot>();
        var constructors = await cTask;
        var drivers = await dTask;
        if (constructors.status != ApiRequestStatus.Success && drivers.status != ApiRequestStatus.Success)
            return;
        var countries = await _data.LoadJsonResourceAsync<List<CountryData>>();
        DriverStandings.InitStandings(
            drivers.result.Data.StandingsTable.StandingsLists[0].DriverStandings, countries);
        ConstructorStandings.InitStandings(
            constructors.result.Data.StandingsTable.StandingsLists[0].ConstructorStandings, countries);
        FeatureLoading = false;
    }

    public void TogglePointsDiffFromLeader() => 
        PointsDiffFromLeader = !PointsDiffFromLeader;
}