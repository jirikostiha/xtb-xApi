namespace xAPI.Responses
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class MarginLevelResponse : BaseResponse
	{
		private double? balance;
		private double? equity;
		private double? margin;
		private double? margin_free;
		private double? margin_level;
		private string currency;
        private double? credit;

		public MarginLevelResponse(string body) : base(body)
		{
			JSONObject ob = (JSONObject) this.ReturnData;
			this.balance = (double?) ob["balance"];
			this.equity = (double?) ob["equity"];
			this.currency = (string) ob["currency"];
			this.margin = (double?) ob["margin"];
			this.margin_free = (double?) ob["margin_free"];
			this.margin_level = (double?) ob["margin_level"];
            this.credit = (double?) ob["credit"];
		}

		public virtual double? Balance
		{
			get
			{
				return balance;
			}
		}

		public virtual double? Equity
		{
			get
			{
				return equity;
			}
		}

		public virtual double? Margin
		{
			get
			{
				return margin;
			}
		}

		public virtual double? Margin_free
		{
			get
			{
				return margin_free;
			}
		}

		public virtual double? Margin_level
		{
			get
			{
				return margin_level;
			}
		}

		public virtual string Currency
		{
			get
			{
				return currency;
			}
		}

        public virtual double? Credit
        {
            get
            {
                return credit;
            }
        }
	}
}