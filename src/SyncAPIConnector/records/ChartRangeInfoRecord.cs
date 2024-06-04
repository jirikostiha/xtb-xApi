using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records
{

    [DebuggerDisplay("{symbol}")]
    public record ChartRangeInfoRecord
    {
        private string symbol;
        private PERIOD_CODE period;
        private long? start;
        private long? end;
        private long? ticks;

        public ChartRangeInfoRecord(string symbol, PERIOD_CODE period, long? start, long? end, long? ticks)
        {
            this.symbol = symbol;
            this.period = period;
            this.start = start;
            this.end = end;
            this.ticks = ticks;
        }

        public virtual JsonObject toJsonObject()
        {
            JsonObject obj = new()
            {
                { "symbol", symbol },
                { "period", (long?)period.Code },
                { "start", start },
                { "end", end },
                { "ticks", ticks }
            };
            return obj;
        }
    }
}