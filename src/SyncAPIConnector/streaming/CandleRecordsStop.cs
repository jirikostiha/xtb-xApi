using System.Text.Json.Nodes;

namespace xAPI.Streaming
{

    sealed class CandleRecordsStop
    {
        readonly string symbol;

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
