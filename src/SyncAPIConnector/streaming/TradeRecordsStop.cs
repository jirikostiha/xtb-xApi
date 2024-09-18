using System.Text.Json.Nodes;

namespace Xtb.XApi.Streaming;

internal sealed class TradeRecordsStop : ICommand
{
    public const string Name = "stopTrades";

    public string CommandName => Name;

    public override string ToString()
    {
        JsonObject result = new()
        {
            { "command", CommandName }
        };

        return result.ToJsonString();
    }
}