using System.Text.Json.Nodes;

namespace Xtb.XApi.Streaming;

internal sealed class CandleRecordsStop(string symbol) : ICommand
{
    public const string Name = "stopCandles";

    public static readonly string[] RequiredArgs = ["symbol"];

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