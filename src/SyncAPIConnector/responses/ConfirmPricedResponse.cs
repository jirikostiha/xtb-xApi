namespace xAPI.Responses
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class ConfirmPricedResponse : BaseResponse
	{
		private long? newRequestId;

		public ConfirmPricedResponse(string body) : base(body)
		{
			JSONObject ob = (JSONObject) this.ReturnData;
			this.newRequestId = (long?) ob["requestId"];
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