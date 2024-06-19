using System.Text.Json.Nodes;

namespace xAPI.Responses
{

    public class CommissionDefResponse : BaseResponse
    {
        private double? commission;
        private double? rateOfExchange;

        public CommissionDefResponse(string body) : base(body)
        {
            JsonObject rd = this.ReturnData.AsObject();
            this.commission = (double?)rd["commission"];
            this.rateOfExchange = (double?)rd["rateOfExchange"];
        }

        public virtual double? Commission
        {
            get
            {
                return commission;
            }
        }

        public virtual double? RateOfExchange
        {
            get
            {
                return rateOfExchange;
            }
        }
    }
}