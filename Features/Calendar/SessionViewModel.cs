using F1Desktop.Enums;
using F1Desktop.Misc.Extensions;
using F1Desktop.Models.ErgastAPI.Schedule;
using F1Desktop.Services.Local;

namespace F1Desktop.Features.Calendar;

public class SessionViewModel : SessionViewModelBase
{
    public string Name { get; }

    public SessionType Type { get; }

    public SessionViewModel(SessionType type, Session session, GlobalConfigService config) : base(session.DateTime,
        config)
    {
        Type = type;
        Name = type.ToDisplayString();
    }
}