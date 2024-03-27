namespace xAPI.Responses
{
    using System;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class CurrentUserDataResponse : BaseResponse
	{
        private string currency;
        private long? leverage;
        private double? leverageMultiplier;
        private string group;
        private int? companyUnit;
        private string spreadType;
        private bool? ibAccount;

        public CurrentUserDataResponse(string body)
            : base(body)
		{
			JSONObject ob = (JSONObject) this.ReturnData;
            this.currency = (string)ob["currency"];
            this.leverage = (long?)ob["leverage"];
            this.leverageMultiplier = (double?)ob["leverageMultiplier"];
            this.group = (string)ob["group"];
            this.companyUnit = (int?)ob["companyUnit"];
            this.spreadType = (string)ob["spreadType"];
            this.ibAccount = (bool?)ob["ibAccount"];
		}

        public virtual string Currency
        {
            get
            {
                return currency;
            }
        }

        [Obsolete("Use LeverageMultiplier instead")]
		public virtual long? Leverage
		{
			get
			{
				return leverage;
			}
		}

        public virtual double? LeverageMultiplier
        {
            get
            {
                return leverageMultiplier;
            }
        }

        public virtual string Group
        {
            get
            {
                return group;
            }
        }

        public virtual int? CompanyUnit
        {
            get
            {
                return companyUnit;
            }
        }

        public virtual string SpreadType
        {
            get
            {
                return spreadType;
            }
        }

        public virtual bool? IbAccount
        {
            get
            {
                return ibAccount;
            }
        }
	}

}