using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class TickPricesSubscribe : SubscribeBase
    {
        public TickPricesSubscribe(string symbol, string streamSessionId, long? minArrivalTime = null, long? maxLevel = null)
            : base(streamSessionId)
        {
            Symbol = symbol;
            MinArrivalTime = minArrivalTime;
            MaxLevel = maxLevel;
        }

        public string Symbol { get; }

        public long? MinArrivalTime { get; }

        public long? MaxLevel { get; }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "getTickPrices" },
                { "symbol", Symbol }
            };

            if (MinArrivalTime.HasValue)
                result.Add("minArrivalTime", MinArrivalTime);

            if (MaxLevel.HasValue)
                result.Add("maxLevel", MaxLevel);

            result.Add("streamSessionId", StreamSessionId);

            return result.ToJsonString();
        }
    }
}