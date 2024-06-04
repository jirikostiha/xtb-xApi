using System.Text.Json.Nodes;

namespace xAPI.Records
{
    public record RedirectRecord : BaseResponseRecord
    {
        private int mainPort;
        private int streamingPort;
        private string address;

        public void FieldsFromJsonObject(JsonObject value)
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
