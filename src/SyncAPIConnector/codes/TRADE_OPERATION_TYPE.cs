using System.Globalization;

namespace xAPI.Codes;

public class TRADE_OPERATION_TYPE : BaseCode
{
    public const long BUY_CODE = 0;
    public const long SELL_CODE = 1;
    public const long BUY_LIMIT_CODE = 2;
    public const long SELL_LIMIT_CODE = 3;
    public const long BUY_STOP_CODE = 4;
    public const long SELL_STOP_CODE = 5;
    public const long BALANCE_CODE = 6;

    public static readonly TRADE_OPERATION_TYPE BUY = new(BUY_CODE);
    public static readonly TRADE_OPERATION_TYPE SELL = new(SELL_CODE);
    public static readonly TRADE_OPERATION_TYPE BUY_LIMIT = new(BUY_LIMIT_CODE);
    public static readonly TRADE_OPERATION_TYPE SELL_LIMIT = new(SELL_LIMIT_CODE);
    public static readonly TRADE_OPERATION_TYPE BUY_STOP = new(BUY_STOP_CODE);
    public static readonly TRADE_OPERATION_TYPE SELL_STOP = new(SELL_STOP_CODE);
    public static readonly TRADE_OPERATION_TYPE BALANCE = new(BALANCE_CODE);

    public TRADE_OPERATION_TYPE(long code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
         Code switch
         {
             BUY_CODE => "buy",
             SELL_CODE => "sell",
             BUY_LIMIT_CODE => "buy limit",
             SELL_LIMIT_CODE => "sell limit",
             BUY_STOP_CODE => "buy stop",
             SELL_STOP_CODE => "sell stop",
             BALANCE_CODE => "balance",
             _ => Code.ToString(CultureInfo.InvariantCulture),
         };
}