using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Symbol}")]
    public record ChartLastInfoRecord
    {
        public ChartLastInfoRecord(string symbol, PERIOD_CODE period, long? start)
        {
            Symbol = symbol;
            Period = period;
            Start = start;
        }

        public string Symbol { get; init; }

        public PERIOD_CODE Period { get; init; }

        public long? Start { get; init; }

        public virtual JsonObject ToJsonObject()
        {
            JsonObject obj = new()
            {
                { "symbol", Symbol },
                { "period", (long?)Period.Code },
                { "start", Start }
            };

            return obj;
        }
    }
}