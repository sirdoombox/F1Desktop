namespace F1Desktop.Services.Interfaces;

public interface ITimeDebug : ITime
{
    public Action<DateTimeOffset> OneSecondTick { get; }
    public Action<DateTimeOffset> TenSecondTick { get; }
    public DateTimeOffset DebugTime { get; set; }
}