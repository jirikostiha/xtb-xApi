using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

internal sealed class KeepAliveSubscribe : SubscribeCommandBase
{
    public const string Name = "getKeepAlive";

    public KeepAliveSubscribe(string streamSessionId)
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