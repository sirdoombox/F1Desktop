using F1Desktop.Models.ErgastAPI.Schedule;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class RaceViewModel : PropertyChangedBase
{
    public string Name { get; }
    public string Test { get; }
    
    public RaceViewModel(Race raceModel)
    {
        Test = raceModel.Time;
        Name = raceModel.raceName;
    }
}