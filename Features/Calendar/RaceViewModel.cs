using System;
using System.Collections.Generic;
using F1Desktop.Misc;
using F1Desktop.Models.ErgastAPI.Schedule;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class RaceViewModel : PropertyChangedBase
{
    public string Name { get; }
    public int RaceNumber { get; }
    public int TotalRaces { get; }
    public bool IsNextRace { get; }
    public BindableCollection<SessionViewModel> Sessions { get; } = new();
    
    public DateTimeOffset DateTime { get; }

    private readonly Race _race;

    public RaceViewModel(Race race, int totalRaces, bool isNextRace)
    {
        _race = race;
        RaceNumber = race.Round;
        Name = race.RaceName;
        DateTime = race.DateTime;
        TotalRaces = totalRaces;
        IsNextRace = isNextRace;
        SetupWeekend(race.IsSprintWeekend ? race.SprintWeekend : race.NormalWeekend);
    }

    private void SetupWeekend(IReadOnlyDictionary<string,Session> sessions)
    {
        foreach (var session in sessions)
        {
            Sessions.Add(new SessionViewModel(session.Key, session.Value));
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