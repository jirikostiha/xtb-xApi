using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    class KeepAliveSubscribe
    {
        private string streamSessionId;

        public KeepAliveSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            JSONObject result = new JSONObject();
            result.Add("command", "getKeepAlive");
            result.Add("streamSessionId", streamSessionId);
            return result.ToString();
        }
    }
}
