using System.Collections.Generic;

namespace xAPI.Records
{
    using System;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class NewsTopicRecord : BaseResponseRecord
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

        public void FieldsFromJSONObject(JSONObject value)
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