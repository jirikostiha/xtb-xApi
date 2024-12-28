using System;
using System.Diagnostics;
using System.Text.Json.Nodes;


namespace Xtb.XApiClient.Model;

[DebuggerDisplay("{Symbol}")]
public sealed record ChartRangeInfoRecord
{
    public ChartRangeInfoRecord(string symbol, PERIOD period, DateTimeOffset? start, DateTimeOffset? end, int? ticks)
    {
        Symbol = symbol;
        Period = period;
        Start = start;
        End = end;
        Ticks = ticks;
    }

    public string Symbol { get; init; }

    public PERIOD Period { get; init; }

    public DateTimeOffset? Start { get; init; }

    public DateTimeOffset? End { get; init; }

    public int? Ticks { get; init; }

    public JsonObject ToJsonObject()
    {
        JsonObject obj = new()
        {
            { "symbol", Symbol },
            { "period", Period?.Code },
            { "start", Start?.ToUnixTimeMilliseconds() },
            { "end", End?.ToUnixTimeMilliseconds() },
            { "ticks", Ticks }
        };

        return obj;
    }
}