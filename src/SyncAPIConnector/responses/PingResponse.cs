using System.Text.Json.Nodes;

namespace xAPI.Responses
{

    public class PingResponse : BaseResponse
    {
        private long? time;
        private string timeString;

        public PingResponse(string body) : base(body)
        {
            JsonObject ob = this.ReturnData?.AsObject();
        }
    }
}