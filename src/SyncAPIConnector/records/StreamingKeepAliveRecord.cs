using System;
using System.Collections.Generic;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public record StreamingKeepAliveRecord : BaseResponseRecord
    {
        public long? Timestamp
        {
            get;
            set;
        }

        public DateTimeOffset? Timestamp2 => Timestamp is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Timestamp.Value);

        public void FieldsFromJSONObject(JSONObject value)
        {
            this.Timestamp = (long?)value["timestamp"];
        }

        public override string ToString()
        {
            return "StreamingKeepAliveRecord{" +
                "timestamp=" + Timestamp +
                '}';
        }
    }
}
