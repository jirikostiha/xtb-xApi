using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    class NewsSubscribe
    {
        private string streamSessionId;

        public NewsSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            JSONObject result = new JSONObject();
            result.Add("command", "getNews");
            result.Add("streamSessionId", streamSessionId);
            return result.ToString();
        }
    }
}
