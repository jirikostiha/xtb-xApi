using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class CandleRecordsSubscribe : SubscribeBase
    {
        public CandleRecordsSubscribe(string symbol, string streamSessionId)
            : base(streamSessionId)
        {
            Symbol = symbol;
        }

        public string Symbol { get; }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getCandles" },
                { "streamSessionId", StreamSessionId },
                { "symbol", Symbol }
            };
            return result.ToJsonString();
        }
    }
}