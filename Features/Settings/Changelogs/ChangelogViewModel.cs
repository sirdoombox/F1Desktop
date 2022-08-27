namespace F1Desktop.Features.Settings.Changelogs;

public class ChangelogViewModel
{
    public string Version { get; }
    public string[] Changes { get; }

    public ChangelogViewModel(string version, string[] changes)
    {
        Version = version;
        Changes = changes;
    }
}