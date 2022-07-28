using System.Windows;
using F1Desktop.Features.Calendar;
using JetBrains.Annotations;
using Stylet;
using Screen = Stylet.Screen;

namespace F1Desktop.Features.Root;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class RootViewModel : Conductor<IScreen>
{
    public CalendarRootViewModel Calendar { get; }

    private readonly IWindowManager _wm;
    
    public RootViewModel(IWindowManager wm, CalendarRootViewModel calendar)
    {
        Calendar = calendar;
        _wm = wm;
    }
    
    public void OpenWindow(Screen toOpen)
    {
        _wm.ShowWindow(toOpen);
    }

    public void Exit() => Application.Current.Shutdown();
}