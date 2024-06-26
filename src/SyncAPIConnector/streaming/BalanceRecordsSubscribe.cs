using System.Text.Json.Nodes;
namespace xAPI.Streaming
{
    internal sealed class BalanceRecordsSubscribe : SubscribeBase
    {
        public BalanceRecordsSubscribe(string streamSessionId)
            : base(streamSessionId)
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "streamSessionId", StreamSessionId }
            };

            return result.ToJsonString();
        }
    }
}