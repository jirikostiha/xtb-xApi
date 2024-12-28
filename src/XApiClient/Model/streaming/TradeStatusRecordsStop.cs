using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

internal sealed class TradeStatusRecordsStop : ICommand
{
    public const string Name = "stopTradeStatus";

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