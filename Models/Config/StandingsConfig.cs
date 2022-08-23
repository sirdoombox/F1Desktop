using F1Desktop.Attributes;
using F1Desktop.Models.Base;

namespace F1Desktop.Models.Config;

[Filename("Standings.cfg")]
public sealed class StandingsConfig : ConfigBase
{
    public bool PointsDiffFromLeader { get; set; }
    
    public StandingsConfig()
    {
        Default();
    }

    public override void Default()
    {
        PointsDiffFromLeader = false;
    }
}