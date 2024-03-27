namespace xAPI.Responses
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class ProfitCalculationResponse : BaseResponse
	{
		private double? profit;

		public ProfitCalculationResponse(string body) : base(body)
		{
			JSONObject ob = (JSONObject) this.ReturnData;
			this.profit = (double?) ob["profit"];
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