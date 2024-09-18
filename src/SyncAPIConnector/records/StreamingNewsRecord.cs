using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Records;

[DebuggerDisplay("{Key}")]
public record StreamingNewsRecord : IBaseResponseRecord, INewsRecord
{
    public string? Body { get; set; }

    public string? Key { get; set; }

    public string? Title { get; set; }

    public DateTimeOffset? Time { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Body = (string?)value["body"];
        Key = (string?)value["key"];
        Title = (string?)value["title"];

        var time = (long?)value["time"];
        Time = time.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(time.Value) : null;
        Debug.Assert(Time?.ToUnixTimeMilliseconds() == time);
    }
}