namespace xAPI.Codes
{
    public class PROFIT_MODE : BaseCode
    {
        public const long FOREX_CODE = 5;
        public const long CFD_CODE = 6;

        public static readonly PROFIT_MODE FOREX = new PROFIT_MODE(FOREX_CODE);
        public static readonly PROFIT_MODE CFD = new PROFIT_MODE(CFD_CODE);

        public PROFIT_MODE(long code)
            : base(code)
        {
        }
    }
}
