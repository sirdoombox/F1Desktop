using System;
using System.Text.Json.Serialization;

namespace F1Desktop.Models.ErgastAPI.Schedule;

public class Session
{
    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("time")]
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
}