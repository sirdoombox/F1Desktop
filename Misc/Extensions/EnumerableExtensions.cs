using F1Desktop.Features.Calendar;
using Stylet;

namespace F1Desktop.Misc.Extensions;

public static class EnumerableExtensions
{
    public static T GetNextSession<T>(this BindableCollection<T> collection) where T : SessionViewModelBase
    {
        var next = collection.First(x => x.SessionTime > DateTimeOffset.Now);
        next.IsNext = true;
        return next;
    }
}