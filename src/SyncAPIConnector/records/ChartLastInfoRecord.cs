using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records;

[DebuggerDisplay("{Symbol}")]
public record ChartLastInfoRecord
{
    public ChartLastInfoRecord(string symbol, PERIOD period, long? start)
    {
        Symbol = symbol;
        Period = period;
        Start = start.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(start.Value) : null;
    }

    public string Symbol { get; init; }

    public PERIOD Period { get; init; }

    public DateTimeOffset? Start { get; init; }

    public virtual JsonObject ToJsonObject()
    {
        JsonObject obj = new()
        {
            { "symbol", Symbol },
            { "period", Period?.Code },
            { "start", Start?.ToUnixTimeMilliseconds() ?? null }
        };

        return obj;
    }
}