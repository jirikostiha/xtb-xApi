using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Model;

[DebuggerDisplay("{Country}, impact:{Impact}")]
public sealed record CalendarRecord : IBaseResponseRecord
{
    public string? Country { get; set; }

    public string? Current { get; set; }

    public string? Forecast { get; set; }

    public string? Impact { get; set; }

    public string? Period { get; set; }

    public string? Previous { get; set; }

    public string? Title { get; set; }

    public DateTimeOffset? Time { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Country = (string?)value["country"];
        Current = (string?)value["current"];
        Forecast = (string?)value["forecast"];
        Impact = (string?)value["impact"];
        Period = (string?)value["period"];
        Previous = (string?)value["previous"];
        Title = (string?)value["title"];

        var time = (long?)value["time"];
        Time = time.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(time.Value) : null;
        Debug.Assert(Time?.ToUnixTimeMilliseconds() == time);
    }
}