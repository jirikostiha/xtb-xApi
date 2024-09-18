using System.Globalization;

namespace Xtb.XApi.Codes;

public class EXECUTION_TYPE : BaseCode
{
    public const int REQUEST_CODE = 0;
    public const int INSTANT_CODE = 1;
    public const int MARKET_CODE = 2;

    public static readonly EXECUTION_TYPE REQUEST = new(REQUEST_CODE);
    public static readonly EXECUTION_TYPE INSTANT = new(INSTANT_CODE);
    public static readonly EXECUTION_TYPE MARKET = new(MARKET_CODE);

    public EXECUTION_TYPE(int code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
        Code switch
        {
            REQUEST_CODE => "request",
            INSTANT_CODE => "instant",
            MARKET_CODE => "market",
            _ => Code.ToString(CultureInfo.InvariantCulture),
        };
}