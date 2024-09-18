using System;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Streaming;

internal sealed class TickPricesSubscribe : SubscribeCommandBase
{
    public const string Name = "getTickPrices";

    public TickPricesSubscribe(string symbol, string streamSessionId, DateTimeOffset? minArrivalTime = null, int? maxLevel = null)
        : base(streamSessionId)
    {
        Symbol = symbol;
        MinArrivalTime = minArrivalTime;
        MaxLevel = maxLevel;
    }

    public override string CommandName => Name;

    public string Symbol { get; }

    public DateTimeOffset? MinArrivalTime { get; }

    public int? MaxLevel { get; }

    public override string ToString()
    {
        JsonObject result = new()
        {
            { "command", CommandName },
            { "symbol", Symbol },
            { "streamSessionId", StreamSessionId }
        };

        if (MinArrivalTime.HasValue)
            result.Add("minArrivalTime", MinArrivalTime.Value.ToUnixTimeMilliseconds());

        if (MaxLevel.HasValue)
            result.Add("maxLevel", MaxLevel);

        return result.ToJsonString();
    }
}