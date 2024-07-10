using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class TradeStatusRecordsSubscribe
    {
        private readonly string streamSessionId;

        public TradeStatusRecordsSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getTradeStatus" },
                { "streamSessionId", streamSessionId }
            };
            return result.ToJsonString();
        }
    }
}