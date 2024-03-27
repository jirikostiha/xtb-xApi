using xAPI.Codes;

namespace xAPI.Responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class TradeTransactionStatusResponse : BaseResponse
	{
		private double? ask;
		private double? bid;
        private string customComment;
        private string message;
		private long? order;
		private REQUEST_STATUS requestStatus;

		public TradeTransactionStatusResponse(string body) : base(body)
		{
			JSONObject ob = (JSONObject) this.ReturnData;
            this.ask = (double?)ob["ask"];
            this.bid = (double?)ob["bid"];
            this.customComment = (string)ob["customComment"];
            this.message = (string)ob["message"];
			this.order = (long?) ob["order"];
			this.requestStatus = new REQUEST_STATUS((long) ob["requestStatus"]);
		}

		public virtual double? Ask
		{
            get
            {
                return ask;
            }

			set
			{
				this.ask = value;
			}
		}

        public virtual double? Bid
        {
            get
            {
                return bid;
            }
            set
            {
                this.bid = value;
            }
        }

        public virtual string CustomComment
        {
            get { return customComment; }
            set { customComment = value; }
        }

        public virtual string Message
        {
            get
            {
                return message;
            }
            set
            {
                this.message = value;
            }
        }

		public virtual long? Order
		{
            get
            {
                return order;
            }
			set
			{
				this.order = value;
			}
		}

		public virtual REQUEST_STATUS RequestStatus
		{
            get
            {
                return requestStatus;
            }
            set
			{
				this.requestStatus = value;
			}
		}
	}
}