﻿using System.Collections.Generic;

namespace F1Desktop.Models.ErgastAPI.Schedule;

public class RaceTable
{
    public string season { get; set; }
    public List<Race> Races { get; set; }
}