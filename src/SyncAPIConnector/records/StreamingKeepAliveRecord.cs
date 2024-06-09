using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    [DebuggerDisplay("{Timestamp2}")]
    public record StreamingKeepAliveRecord : BaseResponseRecord
    {
        public long? Timestamp
        {
            get;
            set;
        }

        public DateTimeOffset? DateTime => Timestamp is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Timestamp.Value);

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
