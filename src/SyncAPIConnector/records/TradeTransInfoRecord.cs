using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records
{

    [DebuggerDisplay("{Symbol}, order:{Order}, volume:{Volume}")]
    public record TradeTransInfoRecord : ISymbol
    {
        private TRADE_OPERATION_CODE cmd;
        private string customComment;
        private long? expiration;
        private long? order;
        private double? price;
        private double? sl;
        private string symbol;
        private double? tp;
        private TRADE_TRANSACTION_TYPE type;
        private double? volume;

        public TRADE_OPERATION_CODE Cmd { get { return cmd; } set { this.cmd = value; } }
        public string CustomComment { get { return customComment; } set { this.customComment = value; } }
        public long? Expiration { get { return expiration; } set { this.expiration = value; } }
        public long? Order { get { return order; } set { this.order = value; } }
        public double? Price { get { return price; } set { this.price = value; } }
        public double? Sl { get { return sl; } set { this.sl = value; } }
        public string Symbol { get { return symbol; } set { this.symbol = value; } }
        public double? Tp { get { return tp; } set { this.tp = value; } }
        public TRADE_TRANSACTION_TYPE Type { get { return type; } set { this.type = value; } }
        public double? Volume { get { return volume; } set { this.volume = value; } }

        public DateTimeOffset? ExpirationDateTime => Expiration is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Expiration.Value);

        public TradeTransInfoRecord(TRADE_OPERATION_CODE cmd, TRADE_TRANSACTION_TYPE type, double? price, double? sl, double? tp, string symbol, double? volume, long? order, string customComment, long? expiration)
        {
            this.cmd = cmd;
            this.type = type;
            this.price = price;
            this.sl = sl;
            this.tp = tp;
            this.symbol = symbol;
            this.volume = volume;
            this.order = order;
            this.customComment = customComment;
            this.expiration = expiration;
        }

        public virtual JsonObject toJsonObject()
        {
            JsonObject obj = new()
            {
                { "cmd", (long)cmd.Code },
                { "type", (long)type.Code },
                { "price", price },
                { "sl", sl },
                { "tp", tp },
                { "symbol", symbol },
                { "volume", volume },
                { "order", order },
                { "customComment", customComment },
                { "expiration", expiration }
            };
            return obj;
        }

        public override string ToString()
        {
            return "TradeTransInfo [" +
                cmd.ToString() + ", " +
                type.ToString() + ", " +
                price.ToString() + ", " +
                sl.ToString() + ", " +
                tp.ToString() + ", " +
                symbol.ToString() + ", " +
                volume.ToString() +
                order.ToString() + ", " +
                customComment.ToString() + ", " +
                expiration.ToString() + ", " +
                "]";
        }
    }
}