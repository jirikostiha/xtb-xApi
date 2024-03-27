using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    class CandleRecordsSubscribe
    {
        private string symbol;
        private string streamSessionId;
        
        public CandleRecordsSubscribe(string symbol, string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
            this.symbol = symbol;
        }

        public override string ToString()
        {
            JSONObject result = new JSONObject();
            result.Add("command", "getCandles");
            result.Add("streamSessionId", streamSessionId);
            result.Add("symbol", symbol);
            return result.ToString();
        }
    }
}
