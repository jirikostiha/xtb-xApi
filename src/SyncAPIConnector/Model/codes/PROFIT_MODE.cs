using System.Globalization;

namespace Xtb.XApi.Model;

public class PROFIT_MODE : BaseCode
{
    public const int FOREX_CODE = 5;
    public const int CFD_CODE = 6;

    public static readonly PROFIT_MODE FOREX = new(FOREX_CODE);
    public static readonly PROFIT_MODE CFD = new(CFD_CODE);

    public PROFIT_MODE(int code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
        Code switch
        {
            FOREX_CODE => "forex",
            CFD_CODE => "cfd",
            _ => Code.ToString(CultureInfo.InvariantCulture),
        };
}