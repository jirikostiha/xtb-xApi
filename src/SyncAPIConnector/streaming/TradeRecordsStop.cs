using System.Text.Json.Nodes;

namespace xAPI.Streaming
{

    sealed class TradeRecordsStop
    {
        public TradeRecordsStop()
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "stopTrades" }
            };
            return result.ToJsonString();
        }
    }
}
