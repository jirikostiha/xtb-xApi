using System.Collections.Generic;

namespace xAPI.Records
{
    using System;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class StreamingNewsRecord : BaseResponseRecord
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

        public void FieldsFromJSONObject(JSONObject value)
        {
            Body = (string)value["body"];
            Key = (string)value["key"];
            Time = (long?)value["time"];
            Title = (string)value["title"];
        }
    }
}