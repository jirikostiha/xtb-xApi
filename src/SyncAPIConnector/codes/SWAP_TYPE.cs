using System.Globalization;

namespace Xtb.XApi.Codes;

public class SWAP_TYPE : BaseCode
{
    public const int POINTS_CODE = 0;
    public const int DOLLARS_CODE = 1;
    public const int INTEREST_CODE = 2;
    public const int MARGIN_CURRENCY_CODE = 3;

    public static readonly SWAP_TYPE POINTS = new(POINTS_CODE);
    public static readonly SWAP_TYPE DOLLARS = new(DOLLARS_CODE);
    public static readonly SWAP_TYPE INTEREST = new(INTEREST_CODE);
    public static readonly SWAP_TYPE MARGIN_CURRENCY = new(MARGIN_CURRENCY_CODE);

    public SWAP_TYPE(int code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
        Code switch
        {
            POINTS_CODE => "points",
            DOLLARS_CODE => "dollars",
            INTEREST_CODE => "interest",
            MARGIN_CURRENCY_CODE => "margin currency",
            _ => Code.ToString(CultureInfo.InvariantCulture),
        };
}