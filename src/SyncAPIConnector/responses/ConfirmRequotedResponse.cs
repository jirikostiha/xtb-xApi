using System.Text.Json.Nodes;

namespace xAPI.Responses
{

    public class ConfirmRequotedResponse : BaseResponse
    {
        private long? newRequestId;

        public ConfirmRequotedResponse(string body) : base(body)
        {
            JsonObject ob = this.ReturnData.AsObject();
            this.newRequestId = (long?)ob["requestId"];
        }

        public virtual long? NewRequestId
        {
            get
            {
                return newRequestId;
            }
        }
    }

}