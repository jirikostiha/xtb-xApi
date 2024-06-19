using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records
{

    [DebuggerDisplay("{Timestamp2}")]
    public record StreamingKeepAliveRecord : BaseResponseRecord
    {
        public long? Timestamp
        {
            get;
            set;
        }

        public DateTimeOffset? DateTime => Timestamp is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Timestamp.Value);

        public void FieldsFromJsonObject(JsonObject value)
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
