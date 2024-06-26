using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class KeepAliveStop
    {
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
