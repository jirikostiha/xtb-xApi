using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class CandleRecordsStop
    {
        private readonly string symbol;

        public CandleRecordsStop(string symbol)
        {
            this.symbol = symbol;
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "stopCandles" },
                { "symbol", symbol }
            };
            return result.ToJsonString();
        }
    }
}