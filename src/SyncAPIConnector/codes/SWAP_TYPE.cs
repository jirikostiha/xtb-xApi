namespace xAPI.Codes
{
    public class SWAP_TYPE : BaseCode
    {
        public static readonly SWAP_TYPE SWAP_BY_POINTS = new SWAP_TYPE(0L);
        public static readonly SWAP_TYPE SWAP_BY_DOLLARS = new SWAP_TYPE(1L);
        public static readonly SWAP_TYPE SWAP_BY_INTEREST = new SWAP_TYPE(2L);
        public static readonly SWAP_TYPE SWAP_BY_MARGIN_CURRENCY = new SWAP_TYPE(3L);

        public SWAP_TYPE(long code) 
            : base(code) 
        { 
        }
    }
}
