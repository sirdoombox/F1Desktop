using System.Threading.Tasks;
using System.Windows;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.ErgastAPI.Schedule;
using F1Desktop.Services.Interfaces;
using F1Desktop.Services.Local;

namespace F1Desktop.Features.Calendar;

public class RaceViewModel : SessionViewModelBase, IViewAware
{
    public string Name { get; }

    public int RaceNumber { get; }

    public int TotalRaces { get; }

    private RaceContentViewModel _raceContent;
    public RaceContentViewModel RaceContent
    {
        get => _raceContent;
        set => SetAndNotify(ref _raceContent, value);
    }

    private readonly RaceContentViewModel _content;
    private bool _isCollapsing;

    private SessionViewModel _nextSession;
    public SessionViewModel NextSession
    {
        get => _nextSession;
        private set => SetAndNotify(ref _nextSession, value);
    }
    
    public Action OnNextSessionChanged { get; set; }

    public RaceViewModel(Race race, int totalRaces, GlobalConfigService global, ITimeService time) 
        : base(race.DateTime, global, time)
    {
        RaceNumber = race.Round;
        Name = race.RaceName;
        TotalRaces = totalRaces;
        _content = new RaceContentViewModel(race, global, time);
    }

    public void OnExpanded()
    {
        _isCollapsing = false;
        RaceContent = _content;
    }

    public async void OnCollapsed()
    {
        _isCollapsing = true;
        await Task.Delay(1000);
        if (!_isCollapsing) return;
        RaceContent = null;
    }

    public void SetWeekendFinished()
    {
        IsNext = false;
        IsUpcoming = false;
        RaceContent.SetWeekendFinished();
    }

    /// <summary>
    /// Checks to see if the next session should change.
    /// </summary>
    /// <returns>True if the next session has changed.</returns>
    public void UpdateNextSession(DateTimeOffset offsetNow)
    {
        if (NextSession is null)
        {
            NextSession = _content.Sessions.GetNextSession(offsetNow);
            OnNextSessionChanged?.Invoke();
            return;
        }

        if (offsetNow < NextSession.SessionTime) return;
        
        NextSession.IsNext = false;
        NextSession.IsUpcoming = false;
        NextSession = _content.Sessions.GetNextSession(offsetNow);
        OnNextSessionChanged?.Invoke();
    }

    public void AttachView(UIElement view) => View = view;
    public UIElement View { get; private set; }
}