using System.Text.Json.Nodes;

namespace xAPI.Streaming
{

    sealed class KeepAliveSubscribe
    {
        private readonly string streamSessionId;

        public KeepAliveSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getKeepAlive" },
                { "streamSessionId", streamSessionId }
            };
            return result.ToJsonString();
        }
    }
}
