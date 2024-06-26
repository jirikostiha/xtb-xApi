using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class TickPricesStop(string symbol)
    {
        public string Symbol { get; } = symbol;

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "stopTickPrices" },
                { "symbol", Symbol }
            };

            return result.ToJsonString();
        }
    }
}