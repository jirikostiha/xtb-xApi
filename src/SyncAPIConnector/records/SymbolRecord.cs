namespace xAPI.Records
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;
    using System;
    using xAPI.Codes;

	public class SymbolRecord : BaseResponseRecord
	{
		private double? ask;
		private double? bid;
		private string categoryName;
        private long? contractSize;
		private string currency;
        private bool? currencyPair;
        private string currencyProfit;
		private string description;
		private long? expiration;
		private string groupName;
		private double? high;
        private long? initialMargin;
		private long? instantMaxVolume;
        private double? leverage;
		private bool? longOnly;
        private double? lotMax;
        private double? lotMin;
        private double? lotStep;
		private double? low;
        private long? marginHedged;
        private bool? marginHedgedStrong;
        private long? marginMaintenance;
        private MARGIN_MODE marginMode;
		private long? precision;
        private double? percentage;
        private PROFIT_MODE profitMode;
        private long? quoteId;
        private double? spreadRaw;
        private double? spreadTable;
		private long? starting;
		private long? stepRuleId;
        private long? stopsLevel;
        private bool? swapEnable;
        private double? swapLong;
        private double? swapShort;
        private SWAP_TYPE swapType;
        private SWAP_ROLLOVER_TYPE swapRollover;
		private string symbol;
        private double? tickSize;
        private double? tickValue;
		private long? time;
        private string timeString;
		private long? type;

		public SymbolRecord()
		{
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

		public virtual string CategoryName
		{
			get
			{
				return categoryName;
			}
			set
			{
				this.categoryName = value;
			}
		}

        public virtual long? ContractSize
        {
			get
			{
				return contractSize;
			}
			set
			{
				this.contractSize = value;
			}
		}

		public virtual string Currency
		{
			get
			{
				return currency;
			}
			set
			{
				this.currency = value;
			}
		}

        public virtual bool? CurrencyPair
        {
            get
            {
                return currencyPair;
            }
            set
            {
                this.currencyPair = value;
            }
        }

        [Obsolete("Use Precision instead")]
        public virtual long? Digits
        {
            get { return Precision; }
        }

        public string CurrencyProfit
        {
            get 
            { 
                return currencyProfit; 
            }
            set 
            { 
                currencyProfit = value; 
            }
        }

		public virtual string Description
		{
			get
			{
				return description;
			}
			set
			{
				this.description = value;
			}
		}

		public virtual long? Expiration
		{
			get
			{
				return expiration;
			}
			set
			{
				this.expiration = value;
			}
		}

		public virtual string GroupName
		{
			get
			{
				return groupName;
			}
			set
			{
				this.groupName = value;
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

        public virtual long? InitialMargin
        {
			get
			{
				return initialMargin;
			}
			set
			{
				this.initialMargin = value;
			}
		}

		public virtual long? InstantMaxVolume
		{
			get
			{
				return instantMaxVolume;
			}
			set
			{
				this.instantMaxVolume = value;
			}
		}

        public virtual double? Leverage
        {
            get
            {
                return leverage;
            }
            set
            {
                this.leverage = value;
            }
        }

		public virtual bool? LongOnly
		{
			get
			{
				return longOnly;
			}
			set
			{
				this.longOnly = value;
			}
		}

        public virtual double? LotMax
		{
			get
			{
				return lotMax;
			}
			set
			{
				this.lotMax = value;
			}
		}

        public virtual double? LotMin
		{
			get
			{
				return lotMin;
			}
			set
			{
				this.lotMin = value;
			}
		}

        public virtual double? LotStep
        {
			get
			{
				return lotStep;
			}
			set
			{
				this.lotStep = value;
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

        public virtual long? MarginHedged
        {
			get
			{
				return marginHedged;
			}
			set
			{
				this.marginHedged = value;
			}
		}

        public virtual bool? MarginHedgedStrong
        {
			get
			{
				return marginHedgedStrong;
			}
			set
			{
				this.marginHedgedStrong = value;
			}
		}

        public virtual long? MarginMaintenance
        {
			get
			{
				return marginMaintenance;
			}
			set
			{
				this.marginMaintenance = value;
			}
		}

        public virtual MARGIN_MODE MarginMode
        {
			get
			{
				return marginMode;
			}
			set
			{
				this.marginMode = value;
			}
		}

		public virtual long? Precision
		{
			get
			{
				return precision;
			}
			set
			{
				this.precision = value;
			}
		}

        public virtual double? Percentage
        {
			get
			{
				return percentage;
			}
			set
			{
				this.percentage = value;
			}
		}

        public virtual PROFIT_MODE ProfitMode
        {
			get
			{
				return profitMode;
			}
			set
			{
				this.profitMode = value;
			}
		}

        public long? QuoteId
        {
            get 
            { 
                return quoteId; 
            }
            set 
            { 
                quoteId = value; 
            }
        }

        public virtual double? SpreadRaw
        {
            get
            {
                return spreadRaw;
            }
            set
            {
                this.spreadTable = value;
            }
        }

        public virtual double? SpreadTable
        {
            get
            {
                return spreadTable;
            }
            set
            {
                this.spreadTable = value;
            }
        }

		public virtual long? Starting
		{
			get
			{
				return starting;
			}
			set
			{
				this.starting = value;
			}
		}
		
		public virtual long? StepRuleId
		{
			get
			{
				return stepRuleId;
			}
			set
			{
				this.stepRuleId = value;
			}
		}
		
		public virtual long? StopsLevel
		{
			get
			{
				return stopsLevel;
			}
			set
			{
				this.stopsLevel = value;
			}
		}

        public virtual bool? SwapEnable
        {
			get
			{
				return swapEnable;
			}
			set
			{
				this.swapEnable = value;
			}
		}

        public virtual double? SwapLong
        {
			get
			{
				return swapLong;
			}
			set
			{
				this.swapLong = value;
			}
		}

        public virtual double? SwapShort
        {
			get
			{
				return swapShort;
			}
			set
			{
				this.swapShort = value;
			}
		}

        public virtual SWAP_TYPE SwapType
        {
			get
			{
				return swapType;
			}
			set
			{
				this.swapType = value;
			}
		}

        public virtual SWAP_ROLLOVER_TYPE SwapRollover
        {
			get
			{
				return swapRollover;
			}
			set
			{
				this.swapRollover = value;
			}
		}

		public virtual string Symbol
		{
			get
			{
				return symbol;
			}
			set
			{
				this.symbol = value;
			}
		}

        public virtual double? TickSize
        {
			get
			{
				return tickSize;
			}
			set
			{
				this.tickSize = value;
			}
		}

        public virtual double? TickValue
        {
			get
			{
				return tickValue;
			}
			set
			{
				this.tickValue = value;
			}
		}

        public virtual long? Time
        {
            get
            {
                return time;
            }
            set
            {
                this.time = value;
            }
        }

        public virtual string TimeString
        {
            get
            {
                return timeString;
            }
            set
            {
                this.timeString = value;
            }
        }

		public virtual long? Type
		{
			get
			{
				return type;
			}
			set
			{
				this.type = value;
			}
		}

        public void FieldsFromJSONObject(JSONObject value)
        {
            this.Ask = (double?)value["ask"];
            this.Bid = (double?)value["bid"];
            this.CategoryName = (string)value["categoryName"];
            this.Currency = (string)value["currency"];
            this.CurrencyPair = (bool?)value["currencyPair"];
            this.CurrencyProfit = (string)value["currencyProfit"];
            this.Description = (string)value["description"];
            this.Expiration = (long?)value["expiration"];
            this.GroupName = (string)value["groupName"];
            this.High = (double?)value["high"];
            this.InstantMaxVolume = (long?)value["instantMaxVolume"];
            this.Leverage = (double)value["leverage"];
            this.LongOnly = (bool?)value["longOnly"];
            this.LotMax = (double?)value["lotMax"];
            this.LotMin = (double?)value["lotMin"];
            this.LotStep = (double?)value["lotStep"];
            this.Low = (double?)value["low"];
            this.Precision = (long?)value["precision"];
            this.Starting = (long?)value["starting"];
            this.StopsLevel = (long?)value["stopsLevel"];
            this.Symbol = (string)value["symbol"];
            this.Time = (long?)value["time"];
            this.TimeString = (string)value["timeString"];
            this.Type = (long?)value["type"];
            this.ContractSize = (long?) value["contractSize"];
            this.InitialMargin = (long?)value["initialMargin"];
            this.MarginHedged = (long?)value["marginHedged"];
            this.MarginHedgedStrong = (bool?) value["marginHedgedStrong"];
            this.MarginMaintenance = (long?)value["marginMaintenance"];
            this.MarginMode = new MARGIN_MODE((long)value["marginMode"]);
            this.Percentage = (double?)value["percentage"];
            this.ProfitMode = new PROFIT_MODE((long)value["profitMode"]);
            this.QuoteId = (long?)value["quoteId"];
            this.SpreadRaw = (double?)value["spreadRaw"];
            this.SpreadTable = (double?)value["spreadTable"];
			this.StepRuleId = (long?)value["stepRuleId"];
            this.SwapEnable = (bool?)value["swapEnable"];
            this.SwapLong = (double?)value["swapLong"];
            this.SwapShort = (double?)value["swapShort"];
            this.SwapType = new SWAP_TYPE((long) value["swapType"]);
            this.SwapRollover = new SWAP_ROLLOVER_TYPE((long)value["swap_rollover3days"]);
            this.TickSize = (double?)value["tickSize"];
            this.TickValue = (double?)value["tickValue"];
        }
    }
}