using System.Globalization;

namespace xAPI.Codes;

public class PERIOD_CODE : BaseCode
{
    public const long PERIOD_M1_CODE = 1;
    public const long PERIOD_M5_CODE = 5;
    public const long PERIOD_M15_CODE = 15;
    public const long PERIOD_M30_CODE = 30;
    public const long PERIOD_H1_CODE = 60;
    public const long PERIOD_H4_CODE = 240;
    public const long PERIOD_D1_CODE = 1440;
    public const long PERIOD_W1_CODE = 10080;
    public const long PERIOD_MN1_CODE = 43200;

    public static readonly PERIOD_CODE PERIOD_M1 = new(PERIOD_M1_CODE);
    public static readonly PERIOD_CODE PERIOD_M5 = new(PERIOD_M5_CODE);
    public static readonly PERIOD_CODE PERIOD_M15 = new(PERIOD_M15_CODE);
    public static readonly PERIOD_CODE PERIOD_M30 = new(PERIOD_M30_CODE);
    public static readonly PERIOD_CODE PERIOD_H1 = new(PERIOD_H1_CODE);
    public static readonly PERIOD_CODE PERIOD_H4 = new(PERIOD_H4_CODE);
    public static readonly PERIOD_CODE PERIOD_D1 = new(PERIOD_D1_CODE);
    public static readonly PERIOD_CODE PERIOD_W1 = new(PERIOD_W1_CODE);
    public static readonly PERIOD_CODE PERIOD_MN1 = new(PERIOD_MN1_CODE);

    public PERIOD_CODE(long code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
        Code switch
        {
            PERIOD_M1_CODE => "M1",
            PERIOD_M5_CODE => "M5",
            PERIOD_M15_CODE => "M15",
            PERIOD_M30_CODE => "M30",
            PERIOD_H1_CODE => "H1",
            PERIOD_H4_CODE => "H4",
            PERIOD_D1_CODE => "D1",
            PERIOD_W1_CODE => "W1",
            PERIOD_MN1_CODE => "MN1",
            _ => Code.ToString(CultureInfo.InvariantCulture),
        };
}