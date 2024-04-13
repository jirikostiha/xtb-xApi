namespace xAPI.Codes
{
    public class SWAP_TYPE : BaseCode
    {
        public const long SWAP_BY_POINTS_CODE = 0;
        public const long SWAP_BY_DOLLARS_CODE = 1;
        public const long SWAP_BY_INTEREST_CODE = 2;
        public const long SWAP_BY_MARGIN_CURRENCY_CODE = 3;

        public static readonly SWAP_TYPE SWAP_BY_POINTS = new SWAP_TYPE(SWAP_BY_POINTS_CODE);
        public static readonly SWAP_TYPE SWAP_BY_DOLLARS = new SWAP_TYPE(SWAP_BY_DOLLARS_CODE);
        public static readonly SWAP_TYPE SWAP_BY_INTEREST = new SWAP_TYPE(SWAP_BY_INTEREST_CODE);
        public static readonly SWAP_TYPE SWAP_BY_MARGIN_CURRENCY = new SWAP_TYPE(SWAP_BY_MARGIN_CURRENCY_CODE);

        public SWAP_TYPE(long code)
            : base(code)
        {
        }
    }
}
