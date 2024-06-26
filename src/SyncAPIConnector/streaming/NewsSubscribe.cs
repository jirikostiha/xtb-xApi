using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class NewsSubscribe : SubscribeBase
    {
        public NewsSubscribe(string streamSessionId)
            : base(streamSessionId)
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getNews" },
                { "streamSessionId", StreamSessionId }
            };

            return result.ToJsonString();
        }
    }
}