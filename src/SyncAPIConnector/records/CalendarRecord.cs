using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records;

[DebuggerDisplay("{Country}, impact:{Impact}")]
public record CalendarRecord : IBaseResponseRecord
{
    public void FieldsFromJsonObject(JsonObject value)
    {
        Country = (string?)value["country"];
        Current = (string?)value["current"];
        Forecast = (string?)value["forecast"];
        Impact = (string?)value["impact"];
        Period = (string?)value["period"];
        Previous = (string?)value["previous"];
        Time = (long?)value["time"];
        Title = (string?)value["title"];
    }

    public string? Country { get; set; }

    public string? Current { get; set; }

    public string? Forecast { get; set; }

    public string? Impact { get; set; }

    public string? Period { get; set; }

    public string? Previous { get; set; }

    public long? Time { get; set; }

    public string? Title { get; set; }

    public DateTimeOffset? DateTime => Time is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Time.Value);
}