using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSONObject = Newtonsoft.Json.Linq.JObject;

namespace xAPI.Records
{
    public class RedirectRecord : BaseResponseRecord
    {
        private int mainPort;
        private int streamingPort;
	    private string address;

        public void FieldsFromJSONObject(JSONObject value)
        {
            this.mainPort = (int)value["mainPort"];
            this.streamingPort = (int)value["streamingPort"];
            this.address = (string)value["address"];
        }

        public int MainPort
        {
            get { return this.mainPort; }
        }

        public int StreamingPort
        {
            get { return this.streamingPort; }
        }

        public string Address
        {
            get { return this.address; }
        }

        public override string ToString()
        {
            return "RedirectRecord [" +
                "mainPort=" + this.mainPort +
                ", streamingPort=" + this.streamingPort +
                ", address=" + this.address + "]";
        }
    }
}
