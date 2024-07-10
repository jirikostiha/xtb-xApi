using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class TradeRecordsSubscribe
    {
        private readonly string streamSessionId;

        public TradeRecordsSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getTrades" },
                { "streamSessionId", streamSessionId }
            };
            return result.ToJsonString();
        }
    }
}