using F1Desktop.Features.Calendar;
using F1Desktop.Services;
using JetBrains.Annotations;
using Stylet;

namespace F1Desktop.Features.Root;

[UsedImplicitly]
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
}