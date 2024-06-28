using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Key}")]
    public record NewsTopicRecord : IBaseResponseRecord, INewsRecord
    {
        public string? Body { get; set; }

        public long? Bodylen { get; set; }

        public string? Key { get; set; }

        public long? Time { get; set; }

        public string? TimeString { get; set; }

        public string? Title { get; set; }

        public DateTimeOffset? DateTime => Time is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Time.Value);

        public void FieldsFromJsonObject(JsonObject value)
        {
            Body = (string?)value["body"];
            Bodylen = (long?)value["bodylen"];
            Key = (string?)value["key"];
            Time = (long?)value["time"];
            TimeString = (string?)value["timeString"];
            Title = (string?)value["title"];
        }
    }
}