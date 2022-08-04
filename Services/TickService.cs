using FluentScheduler;

namespace F1Desktop.Services;

public class TickService
{
    public Action OneSecond { get; set; }
    public Action TenSeconds { get; set; }
    public Action OneMinute { get; set; }

    public TickService()
    {
        JobManager.Initialize();
        JobManager.Start();
        JobManager.AddJob(() => OneSecond?.Invoke(), s => s.ToRunEvery(1).Seconds());
        JobManager.AddJob(() => TenSeconds?.Invoke(), s => s.ToRunEvery(10).Seconds());
        JobManager.AddJob(() => OneMinute?.Invoke(), s => s.ToRunEvery(1).Minutes());
    }
}