using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records;

[DebuggerDisplay("{Symbol}")]
public record ChartRangeInfoRecord
{
    public ChartRangeInfoRecord(string symbol, PERIOD_CODE period, long? start, long? end, long? ticks)
    {
        Symbol = symbol;
        Period = period;
        Start = start.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(start.Value) : null;
        End = end.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(end.Value) : null;
        Ticks = ticks;
    }

    public string Symbol { get; init; }

    public PERIOD_CODE Period { get; init; }

    public DateTimeOffset? Start { get; init; }

    public DateTimeOffset? End { get; init; }

    public long? Ticks { get; init; }

    public virtual JsonObject ToJsonObject()
    {
        JsonObject obj = new()
        {
            { "symbol", Symbol },
            { "period", Period?.Code },
            { "start", Start?.ToUnixTimeMilliseconds() ?? null },
            { "end", End?.ToUnixTimeMilliseconds() ?? null },
            { "ticks", Ticks }
        };

        return obj;
    }
}