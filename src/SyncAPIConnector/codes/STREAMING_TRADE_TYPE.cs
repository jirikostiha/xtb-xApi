using System.Globalization;

namespace xAPI.Codes
{
    public class STREAMING_TRADE_TYPE : BaseCode
    {
        public const long OPEN_CODE = 0;
        public const long PENDING_CODE = 1;
        public const long CLOSE_CODE = 2;

        public static readonly STREAMING_TRADE_TYPE OPEN = new(OPEN_CODE);
        public static readonly STREAMING_TRADE_TYPE PENDING = new(PENDING_CODE);
        public static readonly STREAMING_TRADE_TYPE CLOSE = new(CLOSE_CODE);

        public STREAMING_TRADE_TYPE(long code)
            : base(code)
        {
        }

        /// <summary> Converts to human friendly string. </summary>
        public string? ToFriendlyString() =>
            Code switch
            {
                OPEN_CODE => "open",
                PENDING_CODE => "pending",
                CLOSE_CODE => "close",
                _ => Code.ToString(CultureInfo.InvariantCulture),
            };
    }
}