using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class TradeRecordsSubscribe : SubscribeBase
    {
        public TradeRecordsSubscribe(string streamSessionId)
            : base(streamSessionId)
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getTrades" },
                { "streamSessionId", StreamSessionId }
            };

            return result.ToJsonString();
        }
    }
}