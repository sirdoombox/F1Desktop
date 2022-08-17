using System.Windows;

namespace F1Desktop.Features.News;

public class NewsFiltersViewModel : PropertyChangedBase, IViewAware
{
    public List<int> Numbers { get; } = Enumerable.Range(1, 7).ToList();
    
    public UIElement View { get; private set; }
    
    public void AttachView(UIElement view) => View = view;
}