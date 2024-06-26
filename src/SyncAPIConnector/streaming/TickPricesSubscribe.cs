using System.Text.Json.Nodes;

namespace xAPI.Streaming
{
    internal sealed class TickPricesSubscribe : SubscribeCommandBase
    {
        public const string Name = "getTickPrices";

        public TickPricesSubscribe(string symbol, string streamSessionId, long? minArrivalTime = null, long? maxLevel = null)
            : base(streamSessionId)
        {
            Symbol = symbol;
            MinArrivalTime = minArrivalTime;
            MaxLevel = maxLevel;
        }

        public override string CommandName => Name;

        public string Symbol { get; }

        public long? MinArrivalTime { get; }

        public long? MaxLevel { get; }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", CommandName },
                { "symbol", Symbol },
                { "streamSessionId", StreamSessionId }
            };

            if (MinArrivalTime.HasValue)
                result.Add("minArrivalTime", MinArrivalTime);

            if (MaxLevel.HasValue)
                result.Add("maxLevel", MaxLevel);

            return result.ToJsonString();
        }
    }
}