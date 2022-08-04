using System.Windows;
using F1Desktop.Features.Calendar;
using F1Desktop.Features.News;
using JetBrains.Annotations;
using Stylet;
using Screen = Stylet.Screen;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class RootViewModel : Conductor<IScreen>
{
    public CalendarRootViewModel Calendar { get; }
    public NewsRootViewModel News { get; }

    private readonly IWindowManager _wm;
    
    public RootViewModel(IWindowManager wm, CalendarRootViewModel calendar, NewsRootViewModel news)
    {
        Calendar = calendar;
        News = news;
        _wm = wm;
    }
    
    public void OpenWindow(Screen toOpen)
    {
        _wm.ShowWindow(toOpen);
    }

    public void Exit() => Application.Current.Shutdown();
}