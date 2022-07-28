using System;
using System.Collections.Generic;
using F1Desktop.Models.ErgastAPI.Schedule;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class RaceViewModel : PropertyChangedBase
{
    public string Name { get; }
    public int RaceNumber { get; }
    public BindableCollection<SessionViewModel> Sessions { get; } = new();
    
    public DateTimeOffset DateTime { get; }

    public RaceViewModel(Race race)
    {
        RaceNumber = race.Round;
        Name = race.RaceName;
        DateTime = race.DateTime;
        SetupWeekend(race.IsSprintWeekend ? race.SprintWeekend : race.NormalWeekend);
    }

    private void SetupWeekend(IReadOnlyDictionary<string,Session> sessions)
    {
        foreach (var session in sessions)
        {
            Sessions.Add(new SessionViewModel(session.Key, session.Value));
        }
    }
}