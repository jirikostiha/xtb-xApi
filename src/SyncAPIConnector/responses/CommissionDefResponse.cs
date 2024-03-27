namespace xAPI.Responses
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class CommissionDefResponse : BaseResponse
	{
		private double? commission;
		private double? rateOfExchange;

		public CommissionDefResponse(string body) : base(body)
		{
			JSONObject rd = (JSONObject) this.ReturnData;
			this.commission = (double?) rd["commission"];
			this.rateOfExchange = (double?) rd["rateOfExchange"];
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