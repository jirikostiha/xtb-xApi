using System.Text.Json.Nodes;

namespace xAPI.Streaming
{

    sealed class TradeStatusRecordsStop
    {
        public TradeStatusRecordsStop()
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "stopTradeStatus" }
            };
            return result.ToJsonString();
        }
    }
}
