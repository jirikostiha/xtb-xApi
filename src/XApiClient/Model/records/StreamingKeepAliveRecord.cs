using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

[DebuggerDisplay("{Time}")]
public sealed record StreamingKeepAliveRecord : IBaseResponseRecord
{
    public DateTimeOffset? Time { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        var timestamp = (long?)value["timestamp"];
        Time = timestamp.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(timestamp.Value) : null;
        Debug.Assert(Time?.ToUnixTimeMilliseconds() == timestamp);
    }
}