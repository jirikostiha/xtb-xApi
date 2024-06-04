using System.Text.Json.Nodes;

namespace xAPI.Responses
{

    public class ServerTimeResponse : BaseResponse
    {
        private long? time;
        private string timeString;

        public ServerTimeResponse(string body) : base(body)
        {
            JsonObject ob = this.ReturnData.AsObject();
            this.time = (long?)ob["time"];
            this.timeString = (string)ob["timeString"];
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
    }
}