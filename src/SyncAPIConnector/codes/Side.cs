namespace xAPI.Codes
{
    public class Side : BaseCode
    {
        public const int BUY_CODE = 0;
        public const int SELL_CODE = 1;

        /// <summary>
        /// Buy.
        /// </summary>
        public static readonly Side BUY = new Side(BUY_CODE);

        /// <summary>
        /// Sell.
        /// </summary>
        public static readonly Side SELL = new Side(SELL_CODE);

        public Side FromCode(int code)
        {
            if (code == BUY_CODE)
                return BUY;
            else if (code == SELL_CODE)
                return SELL;
            else
                return null;
        }

        private Side(int code)
            : base(code)
        {
        }
    }
}
