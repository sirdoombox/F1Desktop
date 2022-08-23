using System.Threading.Tasks;
using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
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
        set => SetAndNotify(ref _pointsDiffFromLeader, value);
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
    }

    protected override async void OnFeatureFirstOpened()
    {
        var cTask = _api.GetConstructorStandingsAsync();
        var dTask = _api.GetDriverStandingsAsync();
        await Task.WhenAll(cTask, dTask);
        var countries = await _data.LoadJsonResourceAsync<List<CountryData>>();
        DriverStandings.InitStandings(
            dTask.Result.Data.StandingsTable.StandingsLists[0].DriverStandings, countries);
        ConstructorStandings.InitStandings(
            cTask.Result.Data.StandingsTable.StandingsLists[0].ConstructorStandings, countries);
    }
}