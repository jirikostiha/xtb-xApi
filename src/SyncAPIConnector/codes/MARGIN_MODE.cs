using System.Globalization;

namespace xAPI.Codes;

public class MARGIN_MODE : BaseCode
{
    public const long FOREX_CODE = 101;
    public const long CFD_LEVERAGED_CODE = 102;
    public const long CFD_CODE = 103;

    public static readonly MARGIN_MODE FOREX = new(FOREX_CODE);
    public static readonly MARGIN_MODE CFD_LEVERAGED = new(CFD_LEVERAGED_CODE);
    public static readonly MARGIN_MODE CFD = new(CFD_CODE);

    public MARGIN_MODE(long code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
        Code switch
        {
            FOREX_CODE => "forex",
            CFD_LEVERAGED_CODE => "cfd leveraged",
            CFD_CODE => "cfd",
            _ => Code.ToString(CultureInfo.InvariantCulture),
        };
}