using System.Text.Json.Nodes;

namespace xAPI.Streaming
{

    sealed class TickPricesSubscribe
    {
        private readonly string symbol;
        private readonly long? minArrivalTime;
        private readonly long? maxLevel;
        private readonly string streamSessionId;

        public TickPricesSubscribe(string symbol, string streamSessionId, long? minArrivalTime = null, long? maxLevel = null)
        {
            this.symbol = symbol;
            this.minArrivalTime = minArrivalTime;
            this.streamSessionId = streamSessionId;
            this.maxLevel = maxLevel;
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getTickPrices" },
                { "symbol", symbol }
            };

            if (minArrivalTime.HasValue)
                result.Add("minArrivalTime", minArrivalTime);

            if (maxLevel.HasValue)
                result.Add("maxLevel", maxLevel);

            result.Add("streamSessionId", streamSessionId);
            return result.ToJsonString();
        }
    }
}
