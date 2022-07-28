using System.Windows;
using F1Desktop.Features.Calendar;
using F1Desktop.Services;
using JetBrains.Annotations;
using Stylet;
using Screen = Stylet.Screen;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class RootViewModel : Conductor<IScreen>
{
    public CalendarRootViewModel Calendar { get; }

    private readonly IWindowManager _wm;
    
    public RootViewModel(IWindowManager wm, CalendarRootViewModel calendar, NewsRssService rss)
    {
        Calendar = calendar;
        _wm = wm;
        rss.GetFeed().GetAwaiter().GetResult();
    }
    
    public void OpenWindow(Screen toOpen)
    {
        _wm.ShowWindow(toOpen);
    }

    public void Exit() => Application.Current.Shutdown();
}