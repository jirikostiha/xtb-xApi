using System.Text.Json.Nodes;

namespace xAPI.Streaming
{

    sealed class TickPricesStop
    {
        private readonly string symbol;

        public TickPricesStop(string symbol)
        {
            this.symbol = symbol;
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "stopTickPrices" },
                { "symbol", symbol }
            };
            return result.ToJsonString();
        }
    }
}
