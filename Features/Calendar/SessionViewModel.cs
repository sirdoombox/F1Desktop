using F1Desktop.Enums;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.ErgastAPI.Schedule;

namespace F1Desktop.Features.Calendar;

public class SessionViewModel : SessionViewModelBase
{
    public string Name { get; }
    
    public SessionType Type { get; }

    public SessionViewModel(SessionType type, Session session) : base(session.DateTime)
    {
        Type = type;
        Name = type.ToDisplayString();
    }
}