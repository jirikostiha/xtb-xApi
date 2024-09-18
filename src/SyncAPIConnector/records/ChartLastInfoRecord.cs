using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using Xtb.XApi.Codes;

namespace Xtb.XApi.Records;

[DebuggerDisplay("{Symbol}")]
public record ChartLastInfoRecord
{
    public ChartLastInfoRecord(string symbol, PERIOD period, DateTimeOffset? start)
    {
        Symbol = symbol;
        Period = period;
        Start = start;
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