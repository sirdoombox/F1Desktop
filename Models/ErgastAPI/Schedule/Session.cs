using System.Text.Json.Serialization;

namespace F1Desktop.Models.ErgastAPI.Schedule;

public class Session
{
    public string Date { get; set; }
    public string Time { get; set; }

    private DateTimeOffset _dateTime;

    [JsonIgnore]
    public DateTimeOffset DateTime
    {
        get
        {
            if (_dateTime != default) return _dateTime;
            if (!DateTimeOffset.TryParse($"{Date} {Time}", out _dateTime))
                throw new Exception("Unable to parse session DateTime");
            return _dateTime;
        }
    }

    [JsonIgnore]
    public bool IsUpcoming => DateTime >= DateTimeOffset.Now;
}