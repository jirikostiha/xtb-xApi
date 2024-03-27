using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    class TickPricesStop
    {
        private string symbol;

        public TickPricesStop(string symbol)
        {
            this.symbol = symbol;
        }

        public override string ToString()
        {
            JSONObject result = new JSONObject();
            result.Add("command", "stopTickPrices");
            result.Add("symbol", symbol);
            return result.ToString();
        }
    }
}
