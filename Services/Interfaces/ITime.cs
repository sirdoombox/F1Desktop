using F1Desktop.Enums;

namespace F1Desktop.Services.Interfaces;

public interface ITime
{
    public DateTimeOffset OffsetNow { get; }
    
    public void RegisterTickCallback(Every every, Action<DateTimeOffset> callback);
}