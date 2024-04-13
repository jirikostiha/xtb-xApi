namespace xAPI.Codes
{
    public class TRADE_OPERATION_CODE : BaseCode
    {
        public const long BUY_CODE = 0;
        public const long SELL_CODE = 1;
        public const long BUY_LIMIT_CODE = 2;
        public const long SELL_LIMIT_CODE = 3;
        public const long BUY_STOP_CODE = 4;
        public const long SELL_STOP_CODE = 5;
        public const long BALANCE_CODE = 6;

        public static readonly TRADE_OPERATION_CODE BUY = new TRADE_OPERATION_CODE(BUY_CODE);
        public static readonly TRADE_OPERATION_CODE SELL = new TRADE_OPERATION_CODE(SELL_CODE);
        public static readonly TRADE_OPERATION_CODE BUY_LIMIT = new TRADE_OPERATION_CODE(BUY_LIMIT_CODE);
        public static readonly TRADE_OPERATION_CODE SELL_LIMIT = new TRADE_OPERATION_CODE(SELL_LIMIT_CODE);
        public static readonly TRADE_OPERATION_CODE BUY_STOP = new TRADE_OPERATION_CODE(BUY_STOP_CODE);
        public static readonly TRADE_OPERATION_CODE SELL_STOP = new TRADE_OPERATION_CODE(SELL_STOP_CODE);
        public static readonly TRADE_OPERATION_CODE BALANCE = new TRADE_OPERATION_CODE(BALANCE_CODE);

        public TRADE_OPERATION_CODE(long code)
            : base(code)
        {
        }

        public override string ToString()
        {
            return this.Code.ToString();
        }
    }
}