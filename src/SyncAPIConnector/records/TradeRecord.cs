namespace xAPI.Records
{
    using System;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class TradeRecord : BaseResponseRecord
	{
		private double? close_price;
		private long? close_time;
		private bool? closed;
		private long? cmd;
		private string comment;
		private double? commission;
		private double? commission_agent;
        private string customComment;
		private long? digits;
		private long? expiration;
        private string expirationString;
		private double? margin_rate;
		private double? open_price;
		private long? open_time;
		private long? order;
        private long? order2;
        private long? position;
		private double? profit;
		private double? sl;
		private double? storage;
		private string symbol;
		private long? timestamp;
		private double? tp;
		private long? value_date;
		private double? volume;

		public virtual double? Close_price
		{
			get
			{
				return close_price;
			}
		}

		public virtual long? Close_time
		{
			get
			{
				return close_time;
			}
		}

		public virtual bool? Closed
		{
			get
			{
				return closed;
			}
		}

		public virtual long? Cmd
		{
			get
			{
				return cmd;
			}
		}

		public virtual string Comment
		{
			get
			{
				return comment;
			}
		}

		public virtual double? Commission
		{
			get
			{
				return commission;
			}
		}

		public virtual double? Commission_agent
		{
			get
			{
				return commission_agent;
			}
		}

        public virtual string CustomComment
        {
            get { return customComment; }
        }

		public virtual long? Digits
		{
			get
			{
				return digits;
			}
		}

		public virtual long? Expiration
		{
			get
			{
				return expiration;
			}
		}

        public virtual string ExpirationString
        {
            get
            {
                return expirationString;
            }
        }

        [Obsolete]
		public virtual long? Login
		{
			get { return null; }
		}

		public virtual double? Margin_rate
		{
			get
			{
				return margin_rate;
			}
		}

		public virtual double? Open_price
		{
			get
			{
				return open_price;
			}
		}

		public virtual long? Open_time
		{
			get
			{
				return open_time;
			}
		}

		public virtual long? Order
		{
			get
			{
				return order;
			}
		}

        public virtual long? Order2
        {
            get
            {
                return order2;
            }
        }

        public virtual long? Position
        {
            get
            {
                return position;
            }
        }

		public virtual double? Profit
		{
			get
			{
				return profit;
			}
		}

		public virtual double? Sl
		{
			get
			{
				return sl;
			}
		}

        [Obsolete("Not used any more")]
		public virtual long? Spread
		{
			get { return null; }
		}

		public virtual double? Storage
		{
			get
			{
				return storage;
			}
		}

		public virtual string Symbol
		{
			get
			{
				return symbol;
			}
		}

        [Obsolete("Not used any more")]
		public virtual double? Taxes
		{
			get { return null; }
		}

		public virtual long? Timestamp
		{
			get
			{
				return timestamp;
			}
		}

		public virtual double? Tp
		{
			get
			{
				return tp;
			}
		}

		public virtual long? Value_date
		{
			get
			{
				return value_date;
			}
		}

		public virtual double? Volume
		{
			get
			{
				return volume;
			}
		}

        public void FieldsFromJSONObject(JSONObject value)
        {
            this.close_price = (double?)value["close_price"];
            this.close_time = (long?)value["close_time"];
            this.closed = (bool?)value["closed"];
            this.cmd = (long?)value["cmd"];
            this.comment = (string)value["comment"];
            this.commission = (double?)value["commission"];
            this.commission_agent = (double?)value["commission_agent"];
            this.customComment = (string)value["customComment"];
            this.digits = (long?)value["digits"];
            this.expiration = (long?)value["expiration"];
            this.expirationString = (string)value["expirationString"];
            this.margin_rate = (double?)value["margin_rate"];
            this.open_price = (double?)value["open_price"];
            this.open_time = (long?)value["open_time"];
            this.order = (long?)value["order"];
            this.order2 = (long?)value["order2"];
            this.position = (long?)value["position"];
            this.profit = (double?)value["profit"];
            this.sl = (double?)value["sl"];
            this.storage = (double?)value["storage"];
            this.symbol = (string)value["symbol"];
            this.timestamp = (long?)value["timestamp"];
            this.tp = (double?)value["tp"];
            this.value_date = (long?)value["value_date"];
            this.volume = (double?)value["volume"];
        }

        [Obsolete("Method outdated")]
		public bool FieldsFromJSONObject(JSONObject value, string str)
		{
            return false;
		}
        
        public override string ToString()
        {
            return "TradeRecord{" + "close_price=" + close_price + ", close_time=" + close_time + ", closed=" + closed + ", cmd=" + cmd + ", comment=" + comment + ", commission=" + commission + ", commission_agent=" + commission_agent + ", customComment=" + customComment + ", digits=" + digits + ", expiration=" + expiration + ", expirationString=" + expirationString + ", margin_rate=" + margin_rate + ", open_price=" + open_price + ", open_time=" + open_time + ", order=" + order + ", order2=" + Order2 + ", position=" + Position + ", profit=" + profit + ", sl=" + sl + ", storage=" + storage + ", symbol=" + symbol + ", timestamp=" + timestamp + ", tp=" + tp + ", value_date=" + value_date + ", volume=" + volume + '}';
        }
	}
}