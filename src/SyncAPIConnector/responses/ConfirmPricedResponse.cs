using System.Text.Json.Nodes;

namespace xAPI.Responses
{

    public class ConfirmPricedResponse : BaseResponse
    {
        private long? newRequestId;

        public ConfirmPricedResponse(string body) : base(body)
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