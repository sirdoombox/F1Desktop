using System.Collections.Generic;

namespace F1Desktop.Models.ErgastAPI.Schedule;

public class RaceTable
{
    public ushort Season { get; set; }
    public List<Race> Races { get; set; }
}