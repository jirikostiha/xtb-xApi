using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Symbol}, pos:{Position}, o:{Order}, o2:{Order2}")]
    public record StreamingTradeRecord : IBaseResponseRecord, ITradeRecord
    {
        private TRADE_OPERATION_CODE? cmd2;
        private int? digits;

        public double? Close_price { get; set; }

        public long? Close_time { get; set; }

        public bool? Closed { get; set; }

        public long? Cmd { get; set; }

        public TRADE_OPERATION_CODE? Cmd2
        {
            get { return cmd2; }
        }

        public string Comment { get; set; }

        public double? Commission { get; set; }

        public string CustomComment { get; set; }

        public long? Expiration { get; set; }

        public double? Margin_rate { get; set; }

        public double? Open_price { get; set; }

        public long? Open_time { get; set; }

        public long? Order { get; set; }

        public long? Order2 { get; set; }

        public long? Position { get; set; }

        public double? Profit { get; set; }

        public double? Sl { get; set; }

        public string State { get; set; }

        public double? Storage { get; set; }

        public string Symbol { get; set; }

        public double? Tp { get; set; }

        public STREAMING_TRADE_TYPE Type { get; set; }

        public double? Volume { get; set; }

        public long? Digits
        {
            get { return digits; }
            set { digits = (int?)value; }
        }

        public DateTimeOffset? OpenDateTime => Open_time is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Open_time.Value);

        public DateTimeOffset? CloseDateTime => Close_time is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Close_time.Value);

        public DateTimeOffset? ExpirationDateTime => Expiration is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Expiration.Value);

        public void FieldsFromJsonObject(JsonObject value)
        {
            this.Close_price = (double?)value["close_price"];
            this.Close_time = (long?)value["close_time"];
            this.Closed = (bool?)value["closed"];
            this.Cmd = (long?)value["cmd"];
            this.cmd2 = value["cmd"] is not null ? new TRADE_OPERATION_CODE((long)value["cmd"]) : null;
            this.Comment = (string)value["comment"];
            this.Commission = (double?)value["commision"];
            this.CustomComment = (string)value["customComment"];
            this.Expiration = (long?)value["expiration"];
            this.Margin_rate = (double?)value["margin_rate"];
            this.Open_price = (double?)value["open_price"];
            this.Open_time = (long?)value["open_time"];
            this.Order = (long?)value["order"];
            this.Order2 = (long?)value["order2"];
            this.Position = (long?)value["position"];
            this.Profit = (double?)value["profit"];
            this.Type = new STREAMING_TRADE_TYPE((long)value["type"]);
            this.Sl = (double?)value["sl"];
            this.State = (string)value["state"];
            this.Storage = (double?)value["storage"];
            this.Symbol = (string)value["symbol"];
            this.Tp = (double?)value["tp"];
            this.Volume = (double?)value["volume"];
            this.digits = (int?)value["digits"];
        }

        public void UpdateBy(ITradeRecord other)
        {
            Close_price = other.Close_price;
            Close_time = other.Close_time;
            Closed = other.Closed;
            Cmd = other.Cmd;
            Comment = other.Comment;
            Commission = other.Commission;
            CustomComment = other.CustomComment;
            Expiration = other.Expiration;
            Margin_rate = other.Margin_rate;
            Open_price = other.Open_price;
            Open_time = other.Open_time;
            Order = other.Order;
            Order2 = other.Order2;
            Position = other.Position;
            Profit = other.Profit;
            Sl = other.Sl;
            Storage = other.Storage;
            Symbol = other.Symbol;
            Tp = other.Tp;
            Volume = other.Volume;
        }

        public void UpdateBy(StreamingTradeRecord other)
        {
            Close_price = other.Close_price;
            Close_time = other.Close_time;
            Closed = other.Closed;
            Cmd = other.Cmd;
            Comment = other.Comment;
            Commission = other.Commission;
            CustomComment = other.CustomComment;
            Expiration = other.Expiration;
            Margin_rate = other.Margin_rate;
            Open_price = other.Open_price;
            Open_time = other.Open_time;
            Order = other.Order;
            Order2 = other.Order2;
            Position = other.Position;
            Profit = other.Profit;
            Sl = other.Sl;
            State = other.State;
            Storage = other.Storage;
            Symbol = other.Symbol;
            Tp = other.Tp;
            Type = other.Type;
            Volume = other.Volume;
            digits = other.digits;
        }

        public void Reset()
        {
            Close_price = null;
            Close_time = null;
            Closed = null;
            Cmd = null;
            Comment = null;
            Commission = null;
            CustomComment = null;
            Expiration = null;
            Margin_rate = null;
            Open_price = null;
            Open_time = null;
            Order = null;
            Order2 = null;
            Position = null;
            Profit = null;
            Sl = null;
            State = null;
            Storage = null;
            Symbol = null;
            Tp = null;
            Type = null;
            Volume = null;
            digits = null;
        }
    }
}
