using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class ProfitsSubscribe : SubscribeBase
    {
        public ProfitsSubscribe(string streamSessionId)
            : base(streamSessionId)
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getProfits" },
                { "streamSessionId", StreamSessionId }
            };

            return result.ToJsonString();
        }
    }
}