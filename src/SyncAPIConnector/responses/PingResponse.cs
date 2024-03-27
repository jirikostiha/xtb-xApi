namespace xAPI.Responses
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class PingResponse : BaseResponse
	{
		private long? time;
        private string timeString;

		public PingResponse(string body) : base(body)
		{
			JSONObject ob = (JSONObject) this.ReturnData;
		}
	}
}