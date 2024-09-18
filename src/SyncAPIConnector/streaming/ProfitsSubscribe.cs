using System.Text.Json.Nodes;

namespace Xtb.XApi.Streaming;

internal sealed class ProfitsSubscribe : SubscribeCommandBase
{
    public const string Name = "getProfits";

    public ProfitsSubscribe(string streamSessionId)
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