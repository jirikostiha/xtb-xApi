using System.Globalization;

namespace Xtb.XApi.Codes;

public class TRADE_TRANSACTION_TYPE : BaseCode
{
    public const int ORDER_OPEN_CODE = 0;
    public const int ORDER_CLOSE_CODE = 2;
    public const int ORDER_MODIFY_CODE = 3;
    public const int ORDER_DELETE_CODE = 4;

    public static readonly TRADE_TRANSACTION_TYPE ORDER_OPEN = new(ORDER_OPEN_CODE);
    public static readonly TRADE_TRANSACTION_TYPE ORDER_CLOSE = new(ORDER_CLOSE_CODE);
    public static readonly TRADE_TRANSACTION_TYPE ORDER_MODIFY = new(ORDER_MODIFY_CODE);
    public static readonly TRADE_TRANSACTION_TYPE ORDER_DELETE = new(ORDER_DELETE_CODE);

    public TRADE_TRANSACTION_TYPE(int code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
        Code switch
        {
            ORDER_OPEN_CODE => "open",
            ORDER_CLOSE_CODE => "close",
            ORDER_MODIFY_CODE => "modify",
            ORDER_DELETE_CODE => "delete",
            _ => Code.ToString(CultureInfo.InvariantCulture),
        };
}