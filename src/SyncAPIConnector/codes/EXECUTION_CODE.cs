namespace xAPI.Codes
{
    public class EXECUTION_CODE : BaseCode
    {
        public static readonly EXECUTION_CODE EXE_REQUEST = new EXECUTION_CODE(0L);
        public static readonly EXECUTION_CODE EXE_INSTANT = new EXECUTION_CODE(1L);
        public static readonly EXECUTION_CODE EXE_MARKET = new EXECUTION_CODE(2L);

        public EXECUTION_CODE(long code)
            : base(code) 
        { 
        }
    }
}