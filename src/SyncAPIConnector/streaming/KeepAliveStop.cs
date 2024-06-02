using System.Text.Json.Nodes;

namespace xAPI.Streaming
{

    sealed class KeepAliveStop
    {
        public KeepAliveStop()
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "stopKeepAlive" }
            };
            return result.ToJsonString();
        }
    }
}
