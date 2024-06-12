using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Key}")]
    public record NewsTopicRecord : BaseResponseRecord, INewsRecord
    {
        private string body;
        private long? bodylen;
        private string key;
        private long? time;
        private string timeString;
        private string title;

        public NewsTopicRecord()
        {
        }

        public virtual string Body
        {
            get
            {
                return body;
            }
        }

        public virtual long? Bodylen
        {
            get
            {
                return bodylen;
            }
        }

        [Obsolete("Field removed from API")]
        public virtual string Category
        {
            get { return null; }
        }

        public virtual string Key
        {
            get
            {
                return key;
            }
        }

        [Obsolete("Field removed from API")]
        public virtual LinkedList<string> Keywords
        {
            get { return null; }
        }

        [Obsolete("Field removed from API")]
        public virtual long? Priority
        {
            get { return null; }
        }

        [Obsolete("Field removed from API")]
        public virtual bool? Read
        {
            get { return null; }
        }

        public virtual long? Time
        {
            get
            {
                return time;
            }
        }

        public virtual string TimeString
        {
            get
            {
                return timeString;
            }
        }

        public virtual string Title
        {
            get
            {
                return title;
            }
        }

        [Obsolete("Use Title instead")]
        public virtual string Topic
        {
            get { return title; }
        }

        public DateTimeOffset? DateTime => Time is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Time.Value);

        public void FieldsFromJsonObject(JsonObject value)
        {
            this.body = (string)value["body"];
            this.bodylen = (long?)value["bodylen"];
            this.key = (string)value["key"];
            this.time = (long?)value["time"];
            this.timeString = (string)value["timeString"];
            this.title = (string)value["title"];
        }
    }
}