namespace xAPI.Codes
{
    public class EXECUTION_CODE : BaseCode
    {
        public const long EXE_REQUEST_CODE = 0;
        public const long EXE_INSTANT_CODE = 1;
        public const long EXE_MARKET_CODE = 2;

        public static readonly EXECUTION_CODE EXE_REQUEST = new EXECUTION_CODE(EXE_REQUEST_CODE);
        public static readonly EXECUTION_CODE EXE_INSTANT = new EXECUTION_CODE(EXE_INSTANT_CODE);
        public static readonly EXECUTION_CODE EXE_MARKET = new EXECUTION_CODE(EXE_MARKET_CODE);

        public EXECUTION_CODE(long code)
            : base(code)
        {
        }
    }
}