using System.Diagnostics;
using xAPI.Codes;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

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

        public virtual JSONObject toJSONObject()
        {
            JSONObject obj = new JSONObject();
            obj.Add("symbol", symbol);
            obj.Add("period", (long?)period.Code);
            obj.Add("start", start);
            return obj;
        }
    }
}