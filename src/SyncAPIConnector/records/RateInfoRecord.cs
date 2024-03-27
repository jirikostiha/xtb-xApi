namespace xAPI.Records
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class RateInfoRecord : BaseResponseRecord
	{

		private long? ctm;
		private double? open;
        private double? high;
        private double? low;
        private double? close;
		private double? vol;

		public RateInfoRecord()
		{
		}

		public virtual long? Ctm
		{
			get
			{
				return ctm;
			}
			set
			{
				this.ctm = value;
			}
		}

        public virtual double? Open
		{
			get
			{
				return open;
			}
			set
			{
				this.open = value;
			}
		}

        public virtual double? High
		{
			get
			{
				return high;
			}
			set
			{
				this.high = value;
			}
		}

        public virtual double? Low
		{
			get
			{
				return low;
			}
			set
			{
				this.low = value;
			}
		}

        public virtual double? Close
		{
			get
			{
				return close;
			}
			set
			{
				this.close = value;
			}
		}

        public virtual double? Vol
		{
			get
			{
				return vol;
			}
			set
			{
				this.vol = value;
			}
		}

        public void FieldsFromJSONObject(JSONObject value)
        {
            {
                this.Close = (double?)value["close"];
                this.Ctm = (long?)value["ctm"];
                this.High = (double?)value["high"];
                this.Low = (double?)value["low"];
                this.Open = (double?)value["open"];
                this.Vol = (double?)value["vol"];
            }
        }
    }
}