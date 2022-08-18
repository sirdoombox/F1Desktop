using Serilog.Core;
using Squirrel.SimpleSplat;

namespace F1Desktop.Misc.Logging;

public class SquirrelLogger : ILogger
{
    private readonly Logger _log;
    private readonly IReadOnlyDictionary<LogLevel, Action<string, string>> _logMap;

    public SquirrelLogger(Logger log)
    {
        _log = log;
        _logMap = new Dictionary<LogLevel, Action<string, string>>()
        {
            { LogLevel.Info, _log.Information },
            { LogLevel.Debug, _log.Debug },
            { LogLevel.Warn, _log.Warning },
            { LogLevel.Error, _log.Error },
            { LogLevel.Fatal, _log.Fatal }
        };
    }

    public void Write(string message, LogLevel logLevel) => _logMap[logLevel]("{Message}", message);

    public LogLevel Level { get; set; }
}