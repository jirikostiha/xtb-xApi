using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    class BalanceRecordsStop
    {
        public BalanceRecordsStop()
        {
        }

        public override string ToString()
        {
            JSONObject result = new JSONObject();
            result.Add("command", "stopBalance");
            return result.ToString();
        }
    }
}
