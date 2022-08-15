using System.Threading.Tasks;
using F1Desktop.Models.Resources;
using F1Desktop.Services.Local;
using Stylet;

namespace F1Desktop.Features.Settings;

public class CreditsViewModel : PropertyChangedBase
{
    private readonly DataResourceService _resource;

    public BindableCollection<CreditViewModel> Credits { get; } = new();

    public CreditsViewModel(DataResourceService resource)
    {
        _resource = resource;
    }

    public async Task LoadCredits()
    {
        var data = await _resource.LoadJsonResourceAsync<List<CreditData>>();
        Credits.AddRange(data.Select(x => new CreditViewModel(x)));
    }
}