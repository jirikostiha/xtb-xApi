using System.Globalization;

namespace xAPI.Codes;

public class EXECUTION_TYPE : BaseCode
{
    public const long REQUEST_CODE = 0;
    public const long INSTANT_CODE = 1;
    public const long MARKET_CODE = 2;

    public static readonly EXECUTION_TYPE REQUEST = new(REQUEST_CODE);
    public static readonly EXECUTION_TYPE INSTANT = new(INSTANT_CODE);
    public static readonly EXECUTION_TYPE MARKET = new(MARKET_CODE);

    public EXECUTION_TYPE(long code)
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