using NuGet.Versioning;
using Squirrel;

namespace F1Desktop.Models.Misc;

public struct StartupState
{
    public IAppTools AppTools { get; init; }
    public SemanticVersion Version { get; init; }
    public bool FirstRun { get; init; }
    public bool JustUpdated { get; init; }
}