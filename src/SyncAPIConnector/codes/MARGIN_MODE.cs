namespace xAPI.Codes
{
    public class MARGIN_MODE : BaseCode
    {
        public static readonly MARGIN_MODE FOREX = new MARGIN_MODE(101L);
        public static readonly MARGIN_MODE CFD_LEVERAGED = new MARGIN_MODE(102L);
        public static readonly MARGIN_MODE CFD = new MARGIN_MODE(103L);

        public MARGIN_MODE(long code) 
            : base(code) 
        { 
        }
    }
}
