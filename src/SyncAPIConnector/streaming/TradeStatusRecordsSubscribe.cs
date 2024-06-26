using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class TradeStatusRecordsSubscribe : SubscribeCommandBase
    {
        public const string Name = "getTradeStatus";

        public TradeStatusRecordsSubscribe(string streamSessionId)
            : base(streamSessionId)
        {
        }

        public override string CommandName => Name;

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", CommandName },
                { "streamSessionId", StreamSessionId }
            };

            return result.ToJsonString();
        }
    }
}