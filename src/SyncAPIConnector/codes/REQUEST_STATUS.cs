using System.Globalization;

namespace xAPI.Codes
{
    public class REQUEST_STATUS : BaseCode
    {
        public const long ERROR_CODE = 0;
        public const long PENDING_CODE = 1;
        public const long ACCEPTED_CODE = 3;
        public const long REJECTED_CODE = 4;

        public static readonly REQUEST_STATUS ERROR = new(ERROR_CODE);
        public static readonly REQUEST_STATUS PENDING = new(PENDING_CODE);
        public static readonly REQUEST_STATUS ACCEPTED = new(ACCEPTED_CODE);
        public static readonly REQUEST_STATUS REJECTED = new(REJECTED_CODE);

        public REQUEST_STATUS(long code)
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
}