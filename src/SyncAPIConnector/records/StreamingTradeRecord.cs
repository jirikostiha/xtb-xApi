using System.Collections.Generic;
using xAPI.Codes;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class StreamingTradeRecord : BaseResponseRecord
    {
        private double? close_price;
        private long? close_time;
        private bool? closed;
        private long? cmd;
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
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        public double? Commision
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
        public int? Digits
        {
            get { return digits; }
            set { digits = value; }
        }

        public void FieldsFromJSONObject(JSONObject value)
        {
            this.close_price = (double?)value["close_price"];
            this.close_time = (long?)value["close_time"];
            this.closed = (bool?)value["closed"];
            this.cmd = (long)value["cmd"];
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
    }
}
