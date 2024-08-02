using System.Globalization;

namespace xAPI.Codes;

public class Side : BaseCode
{
    public const int BUY_CODE = 0;
    public const int SELL_CODE = 1;

    /// <summary>
    /// Buy.
    /// </summary>
    public static readonly Side BUY = new(BUY_CODE);

    /// <summary>
    /// Sell.
    /// </summary>
    public static readonly Side SELL = new(SELL_CODE);

    public static Side? FromCode(int code)
    {
        if (code == BUY_CODE)
            return BUY;
        else if (code == SELL_CODE)
            return SELL;
        else
            return null;
    }

    private Side(int code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
        Code switch
        {
            BUY_CODE => "buy",
            SELL_CODE => "sell",
            _ => Code.ToString(CultureInfo.InvariantCulture),
        };
}