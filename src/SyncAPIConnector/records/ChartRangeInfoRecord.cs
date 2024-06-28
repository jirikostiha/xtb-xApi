using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records
{

    [DebuggerDisplay("{Symbol}")]
    public record ChartRangeInfoRecord
    {
        public ChartRangeInfoRecord(string symbol, PERIOD_CODE period, long? start, long? end, long? ticks)
        {
            this.Symbol = symbol;
            this.Period = period;
            this.Start = start;
            this.End = end;
            this.Ticks = ticks;
        }

        public string Symbol { get; init; }
        public PERIOD_CODE Period { get; init; }
        public long? Start { get; init; }
        public long? End { get; init; }
        public long? Ticks { get; init; }

        public virtual JsonObject toJsonObject()
        {
            JsonObject obj = new()
            {
                { "symbol", Symbol },
                { "period", (long?)Period.Code },
                { "start", Start },
                { "end", End },
                { "ticks", Ticks }
            };

            return obj;
        }
    }
}