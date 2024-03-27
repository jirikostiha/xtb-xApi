namespace xAPI.Codes
{
    public class Side : BaseCode
    {
        /// <summary>
        /// Buy.
        /// </summary>
        public static readonly Side BUY = new Side(0);

        /// <summary>
        /// Sell.
        /// </summary>
        public static readonly Side SELL = new Side(1);

        public Side FromCode(int code)
        {
            if (code == 0)
                return BUY;
            else if (code == 1)
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
