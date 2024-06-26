using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class TradeStatusRecordsStop
    {
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