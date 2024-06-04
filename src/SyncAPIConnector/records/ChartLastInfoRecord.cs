using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records
{

    [DebuggerDisplay("{symbol}")]
    public record ChartLastInfoRecord
    {
        private string symbol;
        private PERIOD_CODE period;
        private long? start;

        public ChartLastInfoRecord(string symbol, PERIOD_CODE period, long? start)
        {
            this.symbol = symbol;
            this.period = period;
            this.start = start;
        }

        public virtual JsonObject toJsonObject()
        {
            JsonObject obj = new()
            {
                { "symbol", symbol },
                { "period", (long?)period.Code },
                { "start", start }
            };
            return obj;
        }
    }
}