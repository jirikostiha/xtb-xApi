namespace xAPI.Responses
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class ConfirmRequotedResponse : BaseResponse
	{
		private long? newRequestId;

		public ConfirmRequotedResponse(string body) : base(body)
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