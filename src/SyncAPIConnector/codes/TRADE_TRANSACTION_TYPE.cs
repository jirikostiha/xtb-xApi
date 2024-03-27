namespace xAPI.Codes
{
    public class TRADE_TRANSACTION_TYPE : BaseCode
    {
        public static readonly TRADE_TRANSACTION_TYPE ORDER_OPEN = new TRADE_TRANSACTION_TYPE(0L);
        public static readonly TRADE_TRANSACTION_TYPE ORDER_CLOSE = new TRADE_TRANSACTION_TYPE(2L);
        public static readonly TRADE_TRANSACTION_TYPE ORDER_MODIFY = new TRADE_TRANSACTION_TYPE(3L);
        public static readonly TRADE_TRANSACTION_TYPE ORDER_DELETE = new TRADE_TRANSACTION_TYPE(4L);

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