using System.Globalization;

namespace xAPI.Codes;

public class SWAP_ROLLOVER_TYPE : BaseCode
{
    public const long MONDAY_CODE = 0;
    public const long TUESDAY_CODE = 1;
    public const long WEDNESDAY_CODE = 2;
    public const long THURSDAY_CODE = 3;
    public const long FRIDAY_CODE = 4;

    public static readonly SWAP_ROLLOVER_TYPE MONDAY = new(MONDAY_CODE);
    public static readonly SWAP_ROLLOVER_TYPE TUESDAY = new(TUESDAY_CODE);
    public static readonly SWAP_ROLLOVER_TYPE WEDNESDAY = new(WEDNESDAY_CODE);
    public static readonly SWAP_ROLLOVER_TYPE THURSDAY = new(THURSDAY_CODE);
    public static readonly SWAP_ROLLOVER_TYPE FRIDAY = new(FRIDAY_CODE);

    public SWAP_ROLLOVER_TYPE(long code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
        Code switch
        {
            MONDAY_CODE => "monday",
            TUESDAY_CODE => "tuesday",
            WEDNESDAY_CODE => "wednesday",
            THURSDAY_CODE => "thursday",
            FRIDAY_CODE => "friday",
            _ => Code.ToString(CultureInfo.InvariantCulture),
        };
}