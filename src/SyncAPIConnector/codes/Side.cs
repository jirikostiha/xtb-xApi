using System.Globalization;

namespace Xtb.XApi.Codes;

public class SIDE : BaseCode
{
    public const int BUY_CODE = 0;
    public const int SELL_CODE = 1;

    /// <summary>
    /// Buy.
    /// </summary>
    public static readonly SIDE BUY = new(BUY_CODE);

    /// <summary>
    /// Sell.
    /// </summary>
    public static readonly SIDE SELL = new(SELL_CODE);

    public static SIDE? FromCode(int code)
    {
        if (code == BUY_CODE)
            return BUY;
        else if (code == SELL_CODE)
            return SELL;
        else
            return null;
    }

    private SIDE(int code)
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