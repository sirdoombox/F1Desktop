﻿using System.Windows;
using F1Desktop.Misc;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.ErgastAPI.Schedule;
using F1Desktop.Services.Local;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class RaceViewModel : SessionViewModelBase, IViewAware
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
    public List<SessionViewModel> BuiltSessions { get; }
    private bool _hasExpanded;

    public RaceViewModel(Race race, int totalRaces, GlobalConfigService global) : base(race.DateTime, global)
    {
        _race = race;
        RaceNumber = race.Round;
        Name = race.RaceName;
        TotalRaces = totalRaces;

        var weekendOrder = race.IsSprintWeekend 
            ? Constants.SprintWeekendOrder 
            : Constants.NormalWeekendOrder;
        BuiltSessions = weekendOrder.Select(x => new SessionViewModel(x, race.Sessions[x], global)).ToList();
    }

    public void OnExpanded()
    {
        if (_hasExpanded) return;
        Sessions.AddRange(BuiltSessions);
        _hasExpanded = true;
    }
    
    /// <summary>
    /// Checks to see if the next session should change.
    /// </summary>
    /// <returns>True if the next session has changed.</returns>
    public bool UpdateNextSession()
    {
        if (NextSession is null)
        {
            NextSession = BuiltSessions.GetNextSession();
            return true;
        }
        if (DateTimeOffset.Now < NextSession.SessionTime) return false;
        NextSession.IsNext = false;
        NextSession = BuiltSessions.GetNextSession();
        return true;
    }

    public void OpenWiki() => UrlHelper.Open(_race.Url);

    public void OpenMaps() => UrlHelper.OpenMap(_race.Circuit);

    public void OpenWeather() => UrlHelper.OpenWeather(Name);
    
    public void AttachView(UIElement view) => View = view;
    public UIElement View { get; private set; }
}