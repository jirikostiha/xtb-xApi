using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    class CandleRecordsStop
    {
        string symbol;

        public CandleRecordsStop(string symbol)
        {
            this.symbol = symbol;
        }

        public override string ToString()
        {
            JSONObject result = new JSONObject();
            result.Add("command", "stopCandles");
            result.Add("symbol", symbol);
            return result.ToString();
        }
    }
}
