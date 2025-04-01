using System.Globalization;

namespace Xtb.XApi.Client.Model;

public class TRADE_OPERATION_TYPE : BaseCode
{
    public const int BUY_CODE = 0;
    public const int SELL_CODE = 1;
    public const int BUY_LIMIT_CODE = 2;
    public const int SELL_LIMIT_CODE = 3;
    public const int BUY_STOP_CODE = 4;
    public const int SELL_STOP_CODE = 5;
    public const int BALANCE_CODE = 6;

    public static readonly TRADE_OPERATION_TYPE BUY = new(BUY_CODE);
    public static readonly TRADE_OPERATION_TYPE SELL = new(SELL_CODE);
    public static readonly TRADE_OPERATION_TYPE BUY_LIMIT = new(BUY_LIMIT_CODE);
    public static readonly TRADE_OPERATION_TYPE SELL_LIMIT = new(SELL_LIMIT_CODE);
    public static readonly TRADE_OPERATION_TYPE BUY_STOP = new(BUY_STOP_CODE);
    public static readonly TRADE_OPERATION_TYPE SELL_STOP = new(SELL_STOP_CODE);
    public static readonly TRADE_OPERATION_TYPE BALANCE = new(BALANCE_CODE);

    public TRADE_OPERATION_TYPE(int code)
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