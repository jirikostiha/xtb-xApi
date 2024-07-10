using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class NewsSubscribe
    {
        private readonly string streamSessionId;

        public NewsSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getNews" },
                { "streamSessionId", streamSessionId }
            };
            return result.ToJsonString();
        }
    }
}