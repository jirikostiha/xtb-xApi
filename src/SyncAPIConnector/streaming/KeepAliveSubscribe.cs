using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class KeepAliveSubscribe : SubscribeBase
    {
        public KeepAliveSubscribe(string streamSessionId)
            : base(streamSessionId)
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getKeepAlive" },
                { "streamSessionId", StreamSessionId }
            };

            return result.ToJsonString();
        }
    }
}
