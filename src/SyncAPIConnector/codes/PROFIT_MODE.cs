namespace xAPI.Codes
{
    public class PROFIT_MODE : BaseCode
    {
        public static readonly PROFIT_MODE FOREX = new PROFIT_MODE(5L);
	    public static readonly PROFIT_MODE CFD = new PROFIT_MODE(6L);

        public PROFIT_MODE(long code) 
            : base(code) 
        { 
        }
    }
}
