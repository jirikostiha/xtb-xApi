using System.Text.Json.Nodes;

namespace xAPI.Responses
{

    public class MarginTradeResponse : BaseResponse
    {
        private double? margin;

        public MarginTradeResponse(string body) : base(body)
        {
            JsonObject ob = this.ReturnData.AsObject();
            this.margin = (double?)ob["margin"];
        }

        public virtual double? Margin
        {
            get
            {
                return margin;
            }
        }
    }
}