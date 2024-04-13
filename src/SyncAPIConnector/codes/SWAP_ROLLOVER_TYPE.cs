namespace xAPI.Codes
{
    public class SWAP_ROLLOVER_TYPE : BaseCode
    {
        public const long MONDAY_CODE = 0;
        public const long TUESDAY_CODE = 1;
        public const long WEDNSDAY_CODE = 2;
        public const long THURSDAY_CODE = 3;
        public const long FRIDAY_CODE = 4;

        public static readonly SWAP_ROLLOVER_TYPE MONDAY = new SWAP_ROLLOVER_TYPE(MONDAY_CODE);
        public static readonly SWAP_ROLLOVER_TYPE TUESDAY = new SWAP_ROLLOVER_TYPE(TUESDAY_CODE);
        public static readonly SWAP_ROLLOVER_TYPE WEDNSDAY = new SWAP_ROLLOVER_TYPE(WEDNSDAY_CODE);
        public static readonly SWAP_ROLLOVER_TYPE THURSDAY = new SWAP_ROLLOVER_TYPE(THURSDAY_CODE);
        public static readonly SWAP_ROLLOVER_TYPE FRIDAY = new SWAP_ROLLOVER_TYPE(FRIDAY_CODE);

        public SWAP_ROLLOVER_TYPE(long code)
            : base(code)
        {
        }
    }
}
