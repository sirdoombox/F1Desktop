using F1Desktop.Models.ErgastAPI.Schedule;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class RaceViewModel : PropertyChangedBase
{
    public string Name { get; }
    public int RaceNumber { get; }
    
    public RaceViewModel(Race raceModel)
    {
        RaceNumber = raceModel.Round;
        Name = raceModel.RaceName;
    }
}