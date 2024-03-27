namespace xAPI.Codes
{
    public class REQUEST_STATUS : BaseCode
    {
        public static readonly REQUEST_STATUS ERROR = new REQUEST_STATUS(0L);
        public static readonly REQUEST_STATUS PENDING = new REQUEST_STATUS(1L);
        public static readonly REQUEST_STATUS ACCEPTED = new REQUEST_STATUS(3L);
        public static readonly REQUEST_STATUS REJECTED = new REQUEST_STATUS(4L);

        public REQUEST_STATUS(long code) 
            : base(code) 
        { 
        }
    }
}