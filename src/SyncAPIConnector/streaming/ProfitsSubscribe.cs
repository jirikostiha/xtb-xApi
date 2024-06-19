using System.Text.Json.Nodes;

namespace xAPI.Streaming
{

    sealed class ProfitsSubscribe
    {
        private readonly string streamSessionId;

        public ProfitsSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getProfits" },
                { "streamSessionId", streamSessionId }
            };
            return result.ToJsonString();
        }
    }
}
