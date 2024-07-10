using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class BalanceRecordsSubscribe
    {
        private readonly string streamSessionId;

        public BalanceRecordsSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getBalance" },
                { "streamSessionId", streamSessionId }
            };
            return result.ToJsonString();
        }
    }
}