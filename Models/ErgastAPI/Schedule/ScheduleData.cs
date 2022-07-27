using System;
using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.ErgastAPI;

[Filename("Schedule.dat")]
[CacheDuration(days:7)]
public class ScheduleData : CachedDataBase
{
    public MRData MRData { get; set; }
}