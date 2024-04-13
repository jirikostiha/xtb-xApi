namespace xAPI.Codes
{
    public class REQUEST_STATUS : BaseCode
    {
        public const long ERROR_CODE = 0;
        public const long PENDING_CODE = 1;
        public const long ACCEPTED_CODE = 3;
        public const long REJECTED_CODE = 4;

        public static readonly REQUEST_STATUS ERROR = new REQUEST_STATUS(ERROR_CODE);
        public static readonly REQUEST_STATUS PENDING = new REQUEST_STATUS(PENDING_CODE);
        public static readonly REQUEST_STATUS ACCEPTED = new REQUEST_STATUS(ACCEPTED_CODE);
        public static readonly REQUEST_STATUS REJECTED = new REQUEST_STATUS(REJECTED_CODE);

        public REQUEST_STATUS(long code)
            : base(code)
        {
        }
    }
}