namespace xAPI.Responses
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class ServerTimeResponse : BaseResponse
	{
		private long? time;
        private string timeString;

		public ServerTimeResponse(string body) : base(body)
		{
			JSONObject ob = (JSONObject) this.ReturnData;
			this.time = (long?) ob["time"];
            this.timeString = (string)ob["timeString"];
		}

		public virtual long? Time
		{
			get
			{
				return time;
			}
		}

        public virtual string TimeString
        {
            get
            {
                return timeString;
            }
        }
	}
}