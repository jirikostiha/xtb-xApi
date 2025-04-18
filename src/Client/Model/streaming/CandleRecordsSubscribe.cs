﻿using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

internal sealed class CandleRecordsSubscribe : SubscribeCommandBase
{
    public const string Name = "getCandles";

    public CandleRecordsSubscribe(string symbol, string streamSessionId)
        : base(streamSessionId)
    {
        Symbol = symbol;
    }

    public override string CommandName => Name;

    public string Symbol { get; }

    public override string ToString()
    {
        JsonObject result = new()
        {
            { "command", CommandName },
            { "streamSessionId", StreamSessionId },
            { "symbol", Symbol }
        };

        return result.ToJsonString();
    }
}