using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records
{

    [DebuggerDisplay("{Symbol}, order:{Order}, volume:{Volume}")]
    public record TradeTransInfoRecord : ISymbol
    {
        public TRADE_OPERATION_CODE Cmd { get; init; }

        public string CustomComment { get; init; }

        public long? Expiration { get; init; }

        public long? Order { get; init; }

        public double? Price { get; init; }

        public double? Sl { get; init; }

        public string Symbol { get; init; }

        public double? Tp { get; init; }

        public TRADE_TRANSACTION_TYPE Type { get; init; }

        public double? Volume { get; init; }

        public DateTimeOffset? ExpirationDateTime => Expiration is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Expiration.Value);

        public TradeTransInfoRecord(TRADE_OPERATION_CODE cmd, TRADE_TRANSACTION_TYPE type, double? price, double? sl, double? tp, string symbol, double? volume, long? order, string customComment, long? expiration)
        {
            this.Cmd = cmd;
            this.Type = type;
            this.Price = price;
            this.Sl = sl;
            this.Tp = tp;
            this.Symbol = symbol;
            this.Volume = volume;
            this.Order = order;
            this.CustomComment = customComment;
            this.Expiration = expiration;
        }

        public virtual JsonObject toJsonObject()
        {
            JsonObject obj = new()
            {
                { "cmd", (long)Cmd.Code },
                { "type", (long)Type.Code },
                { "price", Price },
                { "sl", Sl },
                { "tp", Tp },
                { "symbol", Symbol },
                { "volume", Volume },
                { "order", Order },
                { "customComment", CustomComment },
                { "expiration", Expiration }
            };
            return obj;
        }
    }
}