using F1Desktop.Features.Base;
using F1Desktop.Services;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class CalendarRootViewModel : FeatureRootBase
{
    private ErgastAPIService _api;

    public BindableCollection<RaceViewModel> Races { get; } = new();

    public CalendarRootViewModel(ErgastAPIService api)
    {
        _api = api;
    }

    protected override async void OnActivate()
    {
        var data = await _api.GetScheduleAsync();
        if (data is null) return;
        foreach (var race in data.ScheduleData.RaceTable.Races)
        {
            Races.Add(new RaceViewModel(race));
        }
    }
}