using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records;

[DebuggerDisplay("{DateTime}")]
public record StreamingKeepAliveRecord : IBaseResponseRecord
{
    public long? Timestamp
    {
        get;
        set;
    }

    public DateTimeOffset? DateTime => Timestamp is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Timestamp.Value);

    public void FieldsFromJsonObject(JsonObject value)
    {
        Timestamp = (long?)value["timestamp"];
    }
}