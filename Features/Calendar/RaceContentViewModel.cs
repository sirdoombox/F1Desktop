using F1Desktop.Misc;
using F1Desktop.Models.ErgastAPI.Schedule;
using F1Desktop.Services.Local;

namespace F1Desktop.Features.Calendar;

public class RaceContentViewModel : PropertyChangedBase
{
    public BindableCollection<SessionViewModel> Sessions { get; } = new();

    private readonly Race _race;

    public RaceContentViewModel(Race race, GlobalConfigService global)
    {
        _race = race;
        var weekendOrder = race.IsSprintWeekend
            ? Constants.F1.SprintWeekendOrder
            : Constants.F1.NormalWeekendOrder;
        Sessions.AddRange(weekendOrder.Select(x => new SessionViewModel(x, race.Sessions[x], global)));
    }

    public void SetWeekendFinished()
    {
        foreach (var session in Sessions)
            session.IsNext = session.IsUpcoming = false;
    }

    public void OpenWiki() => UrlHelper.Open(_race.Url);

    public void OpenMaps() => UrlHelper.OpenMap(_race.Circuit);

    public void OpenWeather() => UrlHelper.OpenWeather(_race.RaceName);
}