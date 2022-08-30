namespace F1Desktop.Services.Interfaces;

public interface ITimeServiceDebug : ITimeService
{
    public Action<DateTimeOffset> OneSecondTick { get; }
    public Action<DateTimeOffset> TenSecondTick { get; }
    public DateTimeOffset DebugTime { get; set; }
}