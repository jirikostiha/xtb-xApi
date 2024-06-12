using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Symbol}, pos:{Position}, o:{Order}, o2:{Order2}")]
    public record StreamingTradeRecord : BaseResponseRecord, ITradeRecord
    {
        private double? close_price;
        private long? close_time;
        private bool? closed;
        private long? cmd;
        private TRADE_OPERATION_CODE? cmd2;
        private string comment;
        private double? commision;
        private string customComment;
        private long? expiration;
        private double? margin_rate;
        private double? open_price;
        private long? open_time;
        private long? order;
        private long? order2;
        private long? position;
        private double? profit;
        private double? sl;
        private string state;
        private double? storage;
        private string symbol;
        private double? tp;
        private STREAMING_TRADE_TYPE type;
        private double? volume;
        private int? digits;

        public double? Close_price
        {
            get { return close_price; }
            set { close_price = value; }
        }
        public long? Close_time
        {
            get { return close_time; }
            set { close_time = value; }
        }
        public bool? Closed
        {
            get { return closed; }
            set { closed = value; }
        }
        public long? Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }
        public TRADE_OPERATION_CODE? Cmd2
        {
            get { return cmd2; }
        }
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        public double? Commission
        {
            get { return commision; }
            set { commision = value; }
        }
        public string CustomComment
        {
            get { return customComment; }
            set { customComment = value; }
        }
        public long? Expiration
        {
            get { return expiration; }
            set { expiration = value; }
        }
        public double? Margin_rate
        {
            get { return margin_rate; }
            set { margin_rate = value; }
        }
        public double? Open_price
        {
            get { return open_price; }
            set { open_price = value; }
        }
        public long? Open_time
        {
            get { return open_time; }
            set { open_time = value; }
        }
        public long? Order
        {
            get { return order; }
            set { order = value; }
        }
        public long? Order2
        {
            get { return order2; }
            set { order2 = value; }
        }
        public long? Position
        {
            get { return position; }
            set { position = value; }
        }
        public double? Profit
        {
            get { return profit; }
            set { profit = value; }
        }
        public double? Sl
        {
            get { return sl; }
            set { sl = value; }
        }
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        public double? Storage
        {
            get { return storage; }
            set { storage = value; }
        }
        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
        public double? Tp
        {
            get { return tp; }
            set { tp = value; }
        }
        public STREAMING_TRADE_TYPE Type
        {
            get { return type; }
            set { type = value; }
        }
        public double? Volume
        {
            get { return volume; }
            set { volume = value; }
        }
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
            this.close_price = (double?)value["close_price"];
            this.close_time = (long?)value["close_time"];
            this.closed = (bool?)value["closed"];
            this.cmd = (long?)value["cmd"];
            this.cmd2 = value["cmd"] is not null ? new TRADE_OPERATION_CODE((long)value["cmd"]) : null;
            this.comment = (string)value["comment"];
            this.commision = (double?)value["commision"];
            this.customComment = (string)value["customComment"];
            this.expiration = (long?)value["expiration"];
            this.margin_rate = (double?)value["margin_rate"];
            this.open_price = (double?)value["open_price"];
            this.open_time = (long?)value["open_time"];
            this.order = (long?)value["order"];
            this.order2 = (long?)value["order2"];
            this.position = (long?)value["position"];
            this.profit = (double?)value["profit"];
            this.type = new STREAMING_TRADE_TYPE((long)value["type"]);
            this.sl = (double?)value["sl"];
            this.state = (string)value["state"];
            this.storage = (double?)value["storage"];
            this.symbol = (string)value["symbol"];
            this.tp = (double?)value["tp"];
            this.volume = (double?)value["volume"];
            this.digits = (int?)value["digits"];
        }

        public override string ToString()
        {
            return "StreamingTradeRecord{" +
                "symbol=" + symbol +
                ", close_time=" + close_time +
                ", closed=" + closed +
                ", cmd=" + cmd +
                ", comment=" + comment +
                ", commision=" + commision +
                ", customComment=" + customComment +
                ", expiration=" + expiration +
                ", margin_rate=" + margin_rate +
                ", open_price=" + open_price +
                ", open_time=" + open_time +
                ", order=" + order +
                ", order2=" + order2 +
                ", position=" + position +
                ", profit=" + profit +
                ", sl=" + sl +
                ", state=" + state +
                ", storage=" + storage +
                ", symbol=" + symbol +
                ", tp=" + tp +
                ", type=" + type.Code +
                ", volume=" + volume +
                ", digits=" + digits +
                '}';
        }

        public void UpdateBy(ITradeRecord other)
        {
            close_price = other.Close_price;
            close_time = other.Close_time;
            closed = other.Closed;
            cmd = other.Cmd;
            comment = other.Comment;
            commision = other.Commission;
            customComment = other.CustomComment;
            expiration = other.Expiration;
            margin_rate = other.Margin_rate;
            open_price = other.Open_price;
            open_time = other.Open_time;
            order = other.Order;
            order2 = other.Order2;
            position = other.Position;
            profit = other.Profit;
            sl = other.Sl;
            storage = other.Storage;
            symbol = other.Symbol;
            tp = other.Tp;
            volume = other.Volume;
        }

        public void UpdateBy(StreamingTradeRecord other)
        {
            close_price = other.close_price;
            close_time = other.close_time;
            closed = other.closed;
            cmd = other.cmd;
            comment = other.comment;
            commision = other.commision;
            customComment = other.customComment;
            expiration = other.expiration;
            margin_rate = other.margin_rate;
            open_price = other.open_price;
            open_time = other.open_time;
            order = other.order;
            order2 = other.order2;
            position = other.position;
            profit = other.profit;
            sl = other.sl;
            state = other.state;
            storage = other.storage;
            symbol = other.symbol;
            tp = other.tp;
            type = other.type;
            volume = other.volume;
            digits = other.digits;
        }

        public void Reset()
        {
            close_price = null;
            close_time = null;
            closed = null;
            cmd = null;
            comment = null;
            commision = null;
            customComment = null;
            expiration = null;
            margin_rate = null;
            open_price = null;
            open_time = null;
            order = null;
            order2 = null;
            position = null;
            profit = null;
            sl = null;
            state = null;
            storage = null;
            symbol = null;
            tp = null;
            type = null;
            volume = null;
            digits = null;
        }
    }
}
