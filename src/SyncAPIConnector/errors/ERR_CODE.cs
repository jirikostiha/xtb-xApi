namespace Xtb.XApi;

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

    private readonly string _stringCode;

    public ERR_CODE(string code)
    {
        _stringCode = code;
    }

    public virtual string StringValue
    {
        get
        {
            if (_stringCode == null) return "";
            return _stringCode;
        }
    }

    public static string GetErrorDescription(string errorCode)
    {
        return new ERR_CODE(errorCode).getDescription();
    }

    public string getDescription()
    {
        if (string.IsNullOrEmpty(_stringCode)) return string.Empty;
        if (_stringCode.Equals(INVALID_PRICE.StringValue, System.StringComparison.Ordinal)) return "Invalid price.";
        if (_stringCode.Equals(INVALID_SL_TP.StringValue, System.StringComparison.Ordinal)) return "Invalid SL/TP.";
        if (_stringCode.Equals(INVALID_VOLUME.StringValue, System.StringComparison.Ordinal)) return "Invalid volume.";
        if (_stringCode.Equals(LOGIN_DISABLED.StringValue, System.StringComparison.Ordinal)) return "Login disabled.";
        if (_stringCode.Equals(LOGIN_NOT_FOUND.StringValue, System.StringComparison.Ordinal)) return "Login not found.";
        if (_stringCode.Equals(MARKET_IS_CLOSED.StringValue, System.StringComparison.Ordinal)) return "Market is closed!";
        if (_stringCode.Equals(MISMATCHED_PARAMETERS.StringValue, System.StringComparison.Ordinal)) return "Mismatched parameters.";
        if (_stringCode.Equals(MODIFICATION_DENIED.StringValue, System.StringComparison.Ordinal)) return "Modification denied.";
        if (_stringCode.Equals(NOT_ENOUGH_MONEY.StringValue, System.StringComparison.Ordinal)) return "Not enough money!";
        if (_stringCode.Equals(QUOTES_ARE_OFF.StringValue, System.StringComparison.Ordinal)) return "Quotes are off!";
        if (_stringCode.Equals(OPPOSITE_POSITIONS_PROHIBITED.StringValue, System.StringComparison.Ordinal)) return "Opposite positions prohibited!";
        if (_stringCode.Equals(SHORT_POSITIONS_PROHIBITED.StringValue, System.StringComparison.Ordinal)) return "Short positions prohibited!";
        if (_stringCode.Equals(PRICE_HAS_CHANGED.StringValue, System.StringComparison.Ordinal)) return "Price has changed..";
        if (_stringCode.Equals(REQUESTS_TOO_FREQUENT.StringValue, System.StringComparison.Ordinal)) return "Requests too frequent!";
        if (_stringCode.Equals(REQUOTE.StringValue, System.StringComparison.Ordinal)) return "Requote..";
        if (_stringCode.Equals(TOO_MANY_TRADE_REQUESTS.StringValue, System.StringComparison.Ordinal)) return "Too many trade requests!";
        if (_stringCode.Equals(TRADE_IS_DISABLED.StringValue, System.StringComparison.Ordinal)) return "Trade is disabled.";
        if (_stringCode.Equals(TRADE_TIMEOUT.StringValue, System.StringComparison.Ordinal)) return "Trade timeout..";
        if (_stringCode.Equals(SYMBOL_NOT_EXIST_FOR_ACCOUNT.StringValue, System.StringComparison.Ordinal)) return "Symbol not existent for account.";
        if (_stringCode.Equals(CANNOT_TRADE_ON_SYMBOL.StringValue, System.StringComparison.Ordinal)) return "Cannot trade on symbol.";
        if (_stringCode.Equals(CANNOT_CLOSE_PENDING.StringValue, System.StringComparison.Ordinal)) return "Cannot close pending.";
        if (_stringCode.Equals(CANNOT_CLOSE_ALREADY_CLOSED_ORDER.StringValue, System.StringComparison.Ordinal)) return "Cannot close - order already closed.";
        if (_stringCode.Equals(NO_SUCH_TRANSACTION.StringValue, System.StringComparison.Ordinal)) return "No such transaction.";
        if (_stringCode.Equals(UNKNOWN_SYMBOL.StringValue, System.StringComparison.Ordinal)) return "Unknown symbol.";
        if (_stringCode.Equals(UNKNOWN_TRANSACTION_TYPE.StringValue, System.StringComparison.Ordinal)) return "Unknown transaction type.";
        if (_stringCode.Equals(USER_NOT_LOGGED.StringValue, System.StringComparison.Ordinal)) return "User not logged.";
        if (_stringCode.Equals(COMMAND_NOT_EXIST.StringValue, System.StringComparison.Ordinal)) return "Command does not exist.";
        if (_stringCode.Equals(INTERNAL_ERROR.StringValue, System.StringComparison.Ordinal)) return "Internal error.";
        if (_stringCode.Equals(OTHER_ERROR.StringValue, System.StringComparison.Ordinal)) return "Internal error (2).";

        return "Unknown error";
    }
}