namespace xAPI.Codes
{
    public class TRADE_TRANSACTION_TYPE : BaseCode
    {
        public const long ORDER_OPEN_CODE = 0;
        public const long ORDER_CLOSE_CODE = 2;
        public const long ORDER_MODIFY_CODE = 3;
        public const long ORDER_DELETE_CODE = 4;

        public static readonly TRADE_TRANSACTION_TYPE ORDER_OPEN = new TRADE_TRANSACTION_TYPE(ORDER_OPEN_CODE);
        public static readonly TRADE_TRANSACTION_TYPE ORDER_CLOSE = new TRADE_TRANSACTION_TYPE(ORDER_CLOSE_CODE);
        public static readonly TRADE_TRANSACTION_TYPE ORDER_MODIFY = new TRADE_TRANSACTION_TYPE(ORDER_MODIFY_CODE);
        public static readonly TRADE_TRANSACTION_TYPE ORDER_DELETE = new TRADE_TRANSACTION_TYPE(ORDER_DELETE_CODE);

        public TRADE_TRANSACTION_TYPE(long code)
            : base(code)
        {
        }

        public override string ToString()
        {
            return this.Code.ToString();
        }
    }
}