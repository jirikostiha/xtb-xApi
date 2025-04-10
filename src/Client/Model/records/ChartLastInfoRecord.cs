using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

[DebuggerDisplay("{Symbol}")]
public sealed record ChartLastInfoRecord
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

    public JsonObject ToJsonObject()
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