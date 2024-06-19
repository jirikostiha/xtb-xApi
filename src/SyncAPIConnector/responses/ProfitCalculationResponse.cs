using System.Text.Json.Nodes;

namespace xAPI.Responses
{

    public class ProfitCalculationResponse : BaseResponse
    {
        private double? profit;

        public ProfitCalculationResponse(string body) : base(body)
        {
            JsonObject ob = this.ReturnData.AsObject();
            this.profit = (double?)ob["profit"];
        }

        public virtual double? Profit
        {
            get
            {
                return profit;
            }
        }
    }
}