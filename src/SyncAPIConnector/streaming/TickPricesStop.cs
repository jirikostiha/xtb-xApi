using System.Text.Json.Nodes;

namespace Xtb.XApi.Streaming;

internal sealed class TickPricesStop(string symbol) : ICommand
{
    public const string Name = "stopTickPrices";

    public string CommandName => Name;

    public string Symbol { get; } = symbol;

    public override string ToString()
    {
        JsonObject result = new()
        {
            { "command", CommandName },
            { "symbol", Symbol }
        };

        return result.ToJsonString();
    }
}