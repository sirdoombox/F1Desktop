using System.Threading.Tasks;
using F1Desktop.Features.Base;
using F1Desktop.Models.Config;
using F1Desktop.Models.Resources;
using F1Desktop.Services;

namespace F1Desktop.Features.Standings;

public class StandingsRootViewModel : FeatureBaseWithConfig<StandingsConfig>
{
    public StandingsTableViewModel DriverStandings { get; }
    public StandingsTableViewModel ConstructorStandings { get; }
    
    private readonly ErgastAPIService _api;
    private readonly DataResourceService _data;
    
    public StandingsRootViewModel(ConfigService configService, 
        ErgastAPIService api,
        Func<StandingsTableViewModel> standingsTable,
        DataResourceService dataResourceService) 
        : base("Standings", configService)
    {
        _api = api;
        _data = dataResourceService;
        DriverStandings = standingsTable();
        ConstructorStandings = standingsTable();
    }

    protected override async void OnInitialActivate()
    {
        var cTask = _api.GetConstructorStandingsAsync();
        var dTask = _api.GetDriverStandingsAsync();
        await Task.WhenAll(cTask, dTask);
        var countries = await _data.LoadResourceAsync<List<CountryData>>();
        DriverStandings.PassDriverStandings(dTask.Result.Data.StandingsTable.StandingsLists[0].DriverStandings, countries);
        ConstructorStandings.PassConstructorStandings(cTask.Result.Data.StandingsTable.StandingsLists[0].ConstructorStandings, countries);
    }
}