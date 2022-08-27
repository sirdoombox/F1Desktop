using System.Threading.Tasks;
using F1Desktop.Misc.Extensions;
using F1Desktop.Services.Local;

namespace F1Desktop.Features.Settings.Changelogs;

public class ChangelogsViewModel : PropertyChangedBase
{
    public BindableCollection<ChangelogViewModel> Changelogs { get; } = new();

    private readonly DataResourceService _resources;
    
    public ChangelogsViewModel(DataResourceService resources)
    {
        _resources = resources;
    }

    public async Task LoadChangelog()
    {
        var changelogs = await _resources.GetChangelogs(int.MaxValue);
        Changelogs.AddRange(changelogs.Select(x => new ChangelogViewModel(x.ver.ToString(), x.changes)));
    }
    
    public void OnDeactivated(object sender, EventArgs e) => 
        sender.AsWindow().TryClose();
}