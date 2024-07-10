using System.Globalization;

namespace xAPI.Codes;

public class EXECUTION_CODE : BaseCode
{
    public const long EXE_REQUEST_CODE = 0;
    public const long EXE_INSTANT_CODE = 1;
    public const long EXE_MARKET_CODE = 2;

    public static readonly EXECUTION_CODE EXE_REQUEST = new(EXE_REQUEST_CODE);
    public static readonly EXECUTION_CODE EXE_INSTANT = new(EXE_INSTANT_CODE);
    public static readonly EXECUTION_CODE EXE_MARKET = new(EXE_MARKET_CODE);

    public EXECUTION_CODE(long code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
        Code switch
        {
            EXE_REQUEST_CODE => "request",
            EXE_INSTANT_CODE => "instant",
            EXE_MARKET_CODE => "market",
            _ => Code.ToString(CultureInfo.InvariantCulture),
        };
}