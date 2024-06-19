using System.Text.Json.Nodes;

namespace xAPI.Streaming
{

    sealed class NewsStop
    {
        public NewsStop()
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "stopNews" }
            };
            return result.ToJsonString();
        }
    }
}
