﻿using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

internal sealed class NewsSubscribe : SubscribeCommandBase
{
    public const string Name = "getNews";

    public NewsSubscribe(string streamSessionId)
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