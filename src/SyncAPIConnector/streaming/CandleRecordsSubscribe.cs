using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class CandleRecordsSubscribe
    {
        private readonly string symbol;
        private readonly string streamSessionId;

        public CandleRecordsSubscribe(string symbol, string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
            this.symbol = symbol;
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getCandles" },
                { "streamSessionId", streamSessionId },
                { "symbol", symbol }
            };
            return result.ToJsonString();
        }
    }
}