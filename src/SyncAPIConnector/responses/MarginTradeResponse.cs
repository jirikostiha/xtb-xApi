namespace xAPI.Responses
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class MarginTradeResponse : BaseResponse
	{
		private double? margin;

		public MarginTradeResponse(string body) : base(body)
		{
			JSONObject ob = (JSONObject) this.ReturnData;
			this.margin = (double?) ob["margin"];
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