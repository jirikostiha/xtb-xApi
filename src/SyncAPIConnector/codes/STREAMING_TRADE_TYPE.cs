namespace xAPI.Codes
{
    public class STREAMING_TRADE_TYPE : BaseCode
    {
        public const long OPEN_CODE = 0;
        public const long PENDING_CODE = 1;
        public const long CLOSE_CODE = 2;

        public static readonly STREAMING_TRADE_TYPE OPEN = new STREAMING_TRADE_TYPE(OPEN_CODE);
        public static readonly STREAMING_TRADE_TYPE PENDING = new STREAMING_TRADE_TYPE(PENDING_CODE);
        public static readonly STREAMING_TRADE_TYPE CLOSE = new STREAMING_TRADE_TYPE(CLOSE_CODE);

        public STREAMING_TRADE_TYPE(long code)
            : base(code)
        {
        }
    }
}