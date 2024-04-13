namespace xAPI.Codes
{
    public class MARGIN_MODE : BaseCode
    {
        public const long FOREX_CODE = 101;
        public const long CFD_LEVERAGED_CODE = 102;
        public const long CFD_CODE = 103;

        public static readonly MARGIN_MODE FOREX = new MARGIN_MODE(FOREX_CODE);
        public static readonly MARGIN_MODE CFD_LEVERAGED = new MARGIN_MODE(CFD_LEVERAGED_CODE);
        public static readonly MARGIN_MODE CFD = new MARGIN_MODE(CFD_CODE);

        public MARGIN_MODE(long code)
            : base(code)
        {
        }
    }
}
