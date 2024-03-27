namespace xAPI.Errors
{
	public class ERR_CODE
	{
        public static readonly ERR_CODE INVALID_PRICE = new ERR_CODE("BE001");
        public static readonly ERR_CODE INVALID_SL_TP = new ERR_CODE("BE002");
        public static readonly ERR_CODE INVALID_VOLUME = new ERR_CODE("BE003");
        public static readonly ERR_CODE LOGIN_DISABLED = new ERR_CODE("BE004");
        public static readonly ERR_CODE LOGIN_NOT_FOUND = new ERR_CODE("BE005");
        public static readonly ERR_CODE MARKET_IS_CLOSED = new ERR_CODE("BE006");
        public static readonly ERR_CODE MISMATCHED_PARAMETERS = new ERR_CODE("BE007");
        public static readonly ERR_CODE MODIFICATION_DENIED = new ERR_CODE("BE008");
        public static readonly ERR_CODE NOT_ENOUGH_MONEY = new ERR_CODE("BE009");
        public static readonly ERR_CODE QUOTES_ARE_OFF = new ERR_CODE("BE010");
        public static readonly ERR_CODE OPPOSITE_POSITIONS_PROHIBITED = new ERR_CODE("BE011");
        public static readonly ERR_CODE SHORT_POSITIONS_PROHIBITED = new ERR_CODE("BE012");
        public static readonly ERR_CODE PRICE_HAS_CHANGED = new ERR_CODE("BE013");
        public static readonly ERR_CODE REQUESTS_TOO_FREQUENT = new ERR_CODE("BE014");
        public static readonly ERR_CODE REQUOTE = new ERR_CODE("BE015");
        public static readonly ERR_CODE TOO_MANY_TRADE_REQUESTS = new ERR_CODE("BE016");
        public static readonly ERR_CODE TRADE_IS_DISABLED = new ERR_CODE("BE018");
        public static readonly ERR_CODE TRADE_TIMEOUT = new ERR_CODE("BE019");
        public static readonly ERR_CODE SYMBOL_NOT_EXIST_FOR_ACCOUNT = new ERR_CODE("BE094");
        public static readonly ERR_CODE CANNOT_TRADE_ON_SYMBOL = new ERR_CODE("BE095");
        public static readonly ERR_CODE CANNOT_CLOSE_PENDING = new ERR_CODE("BE096");
        public static readonly ERR_CODE CANNOT_CLOSE_ALREADY_CLOSED_ORDER = new ERR_CODE("BE097");
        public static readonly ERR_CODE NO_SUCH_TRANSACTION = new ERR_CODE("BE098");
        public static readonly ERR_CODE UNKNOWN_SYMBOL = new ERR_CODE("BE101");
        public static readonly ERR_CODE UNKNOWN_TRANSACTION_TYPE = new ERR_CODE("BE102");
        public static readonly ERR_CODE USER_NOT_LOGGED = new ERR_CODE("BE103");
        public static readonly ERR_CODE COMMAND_NOT_EXIST = new ERR_CODE("BE104");
        public static readonly ERR_CODE INTERNAL_ERROR = new ERR_CODE("EX001");
        public static readonly ERR_CODE OTHER_ERROR = new ERR_CODE("BE099");

		private string stringCode;

		public ERR_CODE(string code)
		{
			this.stringCode = code;
		}

		public virtual string StringValue
		{
            get
            {
                if (stringCode == null) return "";
                return stringCode;
            }
		}

        public static string getErrorDescription(string errorCode)
        {
            return new ERR_CODE(errorCode).getDescription();
        }

        public string getDescription()
        {
            if (this.stringCode.Equals("")) return "";
            if (this.stringCode.Equals(ERR_CODE.INVALID_PRICE.StringValue)) return "Invalid price.";
            if (this.stringCode.Equals(ERR_CODE.INVALID_SL_TP.StringValue)) return "Invalid SL/TP.";
            if (this.stringCode.Equals(ERR_CODE.INVALID_VOLUME.StringValue)) return "Invalid volume.";
            if (this.stringCode.Equals(ERR_CODE.LOGIN_DISABLED.StringValue)) return "Login disabled.";
            if (this.stringCode.Equals(ERR_CODE.LOGIN_NOT_FOUND.StringValue)) return "Login not found.";
            if (this.stringCode.Equals(ERR_CODE.MARKET_IS_CLOSED.StringValue)) return "Market is closed!";
            if (this.stringCode.Equals(ERR_CODE.MISMATCHED_PARAMETERS.StringValue)) return "Mismatched parameters.";
            if (this.stringCode.Equals(ERR_CODE.MODIFICATION_DENIED.StringValue)) return "Modification denied.";
            if (this.stringCode.Equals(ERR_CODE.NOT_ENOUGH_MONEY.StringValue)) return "Not enough money!";
            if (this.stringCode.Equals(ERR_CODE.QUOTES_ARE_OFF.StringValue)) return "Quotes are off!";
            if (this.stringCode.Equals(ERR_CODE.OPPOSITE_POSITIONS_PROHIBITED.StringValue)) return "Opposite positions prohibited!";
            if (this.stringCode.Equals(ERR_CODE.SHORT_POSITIONS_PROHIBITED.StringValue)) return "Short positions prohibited!";
            if (this.stringCode.Equals(ERR_CODE.PRICE_HAS_CHANGED.StringValue)) return "Price has changed..";
            if (this.stringCode.Equals(ERR_CODE.REQUESTS_TOO_FREQUENT.StringValue)) return "Requests too frequent!";
            if (this.stringCode.Equals(ERR_CODE.REQUOTE.StringValue)) return "Requote..";
            if (this.stringCode.Equals(ERR_CODE.TOO_MANY_TRADE_REQUESTS.StringValue)) return "Too many trade requests!";
            if (this.stringCode.Equals(ERR_CODE.TRADE_IS_DISABLED.StringValue)) return "Trade is disabled.";
            if (this.stringCode.Equals(ERR_CODE.TRADE_TIMEOUT.StringValue)) return "Trade timeout..";
            if (this.stringCode.Equals(ERR_CODE.SYMBOL_NOT_EXIST_FOR_ACCOUNT.StringValue)) return "Symbol not existent for account.";
            if (this.stringCode.Equals(ERR_CODE.CANNOT_TRADE_ON_SYMBOL.StringValue)) return "Cannot trade on symbol.";
            if (this.stringCode.Equals(ERR_CODE.CANNOT_CLOSE_PENDING.StringValue)) return "Cannot close pending.";
            if (this.stringCode.Equals(ERR_CODE.CANNOT_CLOSE_ALREADY_CLOSED_ORDER.StringValue)) return "Cannot close - order already closed.";
            if (this.stringCode.Equals(ERR_CODE.NO_SUCH_TRANSACTION.StringValue)) return "No such transaction.";
            if (this.stringCode.Equals(ERR_CODE.UNKNOWN_SYMBOL.StringValue)) return "Unknown symbol.";
            if (this.stringCode.Equals(ERR_CODE.UNKNOWN_TRANSACTION_TYPE.StringValue)) return "Unknown transaction type.";
            if (this.stringCode.Equals(ERR_CODE.USER_NOT_LOGGED.StringValue)) return "User not logged.";
            if (this.stringCode.Equals(ERR_CODE.COMMAND_NOT_EXIST.StringValue)) return "Command does not exist.";
            if (this.stringCode.Equals(ERR_CODE.INTERNAL_ERROR.StringValue)) return "Internal error.";
            if (this.stringCode.Equals(ERR_CODE.OTHER_ERROR.StringValue)) return "Internal error (2).";

            return "Unknown error";
        }
	}
}