using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records;

[DebuggerDisplay("{Key}")]
public record NewsTopicRecord : IBaseResponseRecord, INewsRecord
{
    public string? Body { get; set; }

    public int? Bodylen { get; set; }

    public string? Key { get; set; }

    public string? Title { get; set; }

    public DateTimeOffset? Time { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Body = (string?)value["body"];
        Bodylen = (int?)value["bodylen"];
        Key = (string?)value["key"];
        Title = (string?)value["title"];

        var time = (long?)value["time"];
        Time = time.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(time.Value) : null;
    }
}