using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    sealed class BalanceRecordsStop
    {
        public BalanceRecordsStop()
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "stopBalance" }
            };
            return result.ToJsonString();
        }
    }
}
