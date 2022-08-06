namespace F1Desktop.Models.ErgastAPI.Shared;

public abstract class MrDataBase
{
    public string Xmlns { get; set; }
    public string Series { get; set; }
    public string Url { get; set; }
    public ushort Limit { get; set; }
    public ushort Offset { get; set; }
    public ushort Total { get; set; }
}