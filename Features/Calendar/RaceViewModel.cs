using System;
using System.Linq;
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
    
    public SessionViewModel NextSession { get; private set; }

    private readonly Race _race;

    public RaceViewModel(Race race, int totalRaces, bool isNextRace) : base(race.DateTime)
    {
        _race = race;
        RaceNumber = race.Round;
        Name = race.RaceName;
        TotalRaces = totalRaces;
        IsNext = isNextRace;
        SetupWeekend(race.IsSprintWeekend ? race.SprintWeekend : race.NormalWeekend);
    }

    private void SetupWeekend(IReadOnlyDictionary<string,Session> sessions)
    {
        foreach (var session in sessions)
        {
            Sessions.Add(new SessionViewModel(session.Key, session.Value));
        }
    }

    public void UpdateNextSession()
    {
        if (NextSession is null)
            NextSession = Sessions.GetNextSession();
        else if (NextSession.SessionTime >= DateTimeOffset.Now)
        {
            NextSession.IsNext = false;
            NextSession = Sessions.GetNextSession();
        }
    }

    public void OpenWiki()
    {
        UrlHelper.Open(_race.Url);
    }

    public void OpenMaps()
    {
        UrlHelper.Open(_race.Circuit);
    }
}