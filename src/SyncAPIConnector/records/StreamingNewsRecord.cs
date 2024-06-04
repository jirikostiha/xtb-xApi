using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Key}")]
    public record StreamingNewsRecord : BaseResponseRecord, INewsRecord
    {
        public StreamingNewsRecord()
        {
        }

        public string Body
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public long? Time
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public DateTimeOffset? Time2 => Time is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Time.Value);

        public void FieldsFromJsonObject(JsonObject value)
        {
            Body = (string)value["body"];
            Key = (string)value["key"];
            Time = (long?)value["time"];
            Title = (string)value["title"];
        }
    }
}