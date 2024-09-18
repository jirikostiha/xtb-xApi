using System.Globalization;

namespace Xtb.XApi.Codes;

public class REQUEST_STATUS : BaseCode
{
    public const int ERROR_CODE = 0;
    public const int PENDING_CODE = 1;
    public const int ACCEPTED_CODE = 3;
    public const int REJECTED_CODE = 4;

    public static readonly REQUEST_STATUS ERROR = new(ERROR_CODE);
    public static readonly REQUEST_STATUS PENDING = new(PENDING_CODE);
    public static readonly REQUEST_STATUS ACCEPTED = new(ACCEPTED_CODE);
    public static readonly REQUEST_STATUS REJECTED = new(REJECTED_CODE);

    public REQUEST_STATUS(int code)
        : base(code)
    {
    }

    /// <summary> Converts to human friendly string. </summary>
    public string? ToFriendlyString() =>
        Code switch
        {
            ERROR_CODE => "error",
            PENDING_CODE => "pending",
            ACCEPTED_CODE => "accepted",
            REJECTED_CODE => "rejected",
            _ => Code.ToString(CultureInfo.InvariantCulture),
        };
}