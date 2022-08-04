using F1Desktop.Misc;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.ErgastAPI.Schedule;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class RaceViewModel : SessionViewModelBase
{
    public string Name { get; }
    
    public int RaceNumber { get; }
    
    public int TotalRaces { get; }
    
    public BindableCollection<SessionViewModel> Sessions { get; } = new();
    
    private SessionViewModel _nextSession;
    public SessionViewModel NextSession
    {
        get => _nextSession;
        private set => SetAndNotify(ref _nextSession, value);
    }
    
    private readonly Race _race;

    public RaceViewModel(Race race, int totalRaces) : base(race.DateTime)
    {
        _race = race;
        RaceNumber = race.Round;
        Name = race.RaceName;
        TotalRaces = totalRaces;
        var weekendOrder = race.IsSprintWeekend 
            ? Constants.SprintWeekendOrder 
            : Constants.NormalWeekendOrder;
        foreach (var session in weekendOrder)
            Sessions.Add(new SessionViewModel(session, race.Sessions[session]));
    }
    
    /// <summary>
    /// Checks to see if the next session should change.
    /// </summary>
    /// <returns>True if the next session has changed.</returns>
    public bool UpdateNextSession()
    {
        if (NextSession is null)
        {
            NextSession = Sessions.GetNextSession();
            return true;
        }
        if (NextSession.SessionTime < DateTimeOffset.Now) return false;
        NextSession.IsNext = false;
        NextSession = Sessions.GetNextSession();
        return true;
    }

    public void OpenWiki() => UrlHelper.Open(_race.Url);

    public void OpenMaps() => UrlHelper.OpenMap(_race.Circuit);

    public void OpenWeather() => UrlHelper.OpenWeather(Name);
}