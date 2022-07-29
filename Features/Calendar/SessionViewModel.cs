using System;
using F1Desktop.Models.ErgastAPI.Schedule;
using Stylet;

namespace F1Desktop.Features.Calendar;

public class SessionViewModel : SessionViewModelBase
{
    public string Name { get; }

    public SessionViewModel(string name, Session session) : base(session.DateTime)
    {
        Name = name;
    }
}