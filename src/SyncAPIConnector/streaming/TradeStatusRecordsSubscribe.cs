using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class TradeStatusRecordsSubscribe : SubscribeBase
    {
        public TradeStatusRecordsSubscribe(string streamSessionId)
            : base(streamSessionId)
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getTradeStatus" },
                { "streamSessionId", StreamSessionId }
            };

            return result.ToJsonString();
        }
    }
}