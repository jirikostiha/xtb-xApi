using System.Globalization;

namespace xAPI.Codes;

public class PERIOD : BaseCode
{
    public const int M1_CODE = 1;
    public const int M5_CODE = 5;
    public const int M15_CODE = 15;
    public const int M30_CODE = 30;
    public const int H1_CODE = 60;
    public const int H4_CODE = 240;
    public const int D1_CODE = 1440;
    public const int W1_CODE = 10080;
    public const int MN1_CODE = 43200;

    public static readonly PERIOD M1 = new(M1_CODE);
    public static readonly PERIOD M5 = new(M5_CODE);
    public static readonly PERIOD M15 = new(M15_CODE);
    public static readonly PERIOD M30 = new(M30_CODE);
    public static readonly PERIOD H1 = new(H1_CODE);
    public static readonly PERIOD H4 = new(H4_CODE);
    public static readonly PERIOD D1 = new(D1_CODE);
    public static readonly PERIOD W1 = new(W1_CODE);
    public static readonly PERIOD MN1 = new(MN1_CODE);

    public PERIOD(int code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
        Code switch
        {
            M1_CODE => "M1",
            M5_CODE => "M5",
            M15_CODE => "M15",
            M30_CODE => "M30",
            H1_CODE => "H1",
            H4_CODE => "H4",
            D1_CODE => "D1",
            W1_CODE => "W1",
            MN1_CODE => "MN1",
            _ => Code.ToString(CultureInfo.InvariantCulture),
        };
}