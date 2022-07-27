using F1Desktop.Services;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class CalendarRootViewModel : Screen
{
    private ErgastAPIService _api;
    
    public CalendarRootViewModel(ErgastAPIService api)
    {
        _api = api;
    }
}