﻿using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

internal sealed class BalanceRecordsSubscribe : SubscribeCommandBase
{
    public const string Name = "getBalance";

    public BalanceRecordsSubscribe(string streamSessionId)
        : base(streamSessionId)
    {
    }

    public override string CommandName => Name;

    public override string ToString()
    {
        JsonObject result = new()
        {
            { "command", CommandName },
            { "streamSessionId", StreamSessionId }
        };

        return result.ToJsonString();
    }
}