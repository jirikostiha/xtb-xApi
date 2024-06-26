using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class ProfitsStop
    {
        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "stopProfits" }
            };

            return result.ToJsonString();
        }
    }
}