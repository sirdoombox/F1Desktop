using System;
using F1Desktop.Models.ErgastAPI.Schedule;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class SessionViewModel : PropertyChangedBase
{
    public string Name { get; }
    public DateTimeOffset DateTime { get; }
    
    public SessionViewModel(string name, Session session)
    {
        Name = name;
        DateTime = session.DateTime;
    }
}