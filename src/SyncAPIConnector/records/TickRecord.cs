using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Symbol}, ask:{Ask}, bid:{Bid}")]
    public record TickRecord : BaseResponseRecord, ITickRecord
    {
        private double? ask;
        private long? askVolume;
        private double? bid;
        private long? bidVolume;
        private double? high;
        private long? level;
        private double? low;
        private double? spreadRaw;
        private double? spreadTable;
        private string symbol;
        private long? timestamp;

        public TickRecord()
        {
        }

        public virtual double? Ask
        {
            get
            {
                return ask;
            }
        }

        public virtual long? AskVolume
        {
            get
            {
                return askVolume;
            }
        }

        public virtual double? Bid
        {
            get
            {
                return bid;
            }
        }

        public virtual long? BidVolume
        {
            get
            {
                return bidVolume;
            }
        }

        public virtual double? High
        {
            get
            {
                return high;
            }
        }

        public virtual long? Level
        {
            get
            {
                return level;
            }
        }

        public virtual double? Low
        {
            get
            {
                return low;
            }
        }

        public virtual double? SpreadRaw
        {
            get
            {
                return spreadRaw;
            }
        }

        public virtual double? SpreadTable
        {
            get
            {
                return spreadTable;
            }
        }

        public virtual string Symbol
        {
            get
            {
                return symbol;
            }
        }

        public virtual long? Timestamp
        {
            get
            {
                return timestamp;
            }
        }

        public DateTimeOffset? DateTime => Timestamp is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Timestamp.Value);

        public void FieldsFromJsonObject(JsonObject value)
        {
            FieldsFromJsonObject(value, null);
        }

        public bool FieldsFromJsonObject(JsonObject value, string str)
        {
            this.ask = (double?)value["ask"];
            this.askVolume = (long?)value["askVolume"];
            this.bid = (double?)value["bid"];
            this.bidVolume = (long?)value["bidVolume"];
            this.high = (double?)value["high"];
            this.level = (long?)value["level"];
            this.low = (double?)value["low"];
            this.spreadRaw = (double?)value["spreadRaw"];
            this.spreadTable = (double?)value["spreadTable"];
            this.symbol = (string)value["symbol"];
            this.timestamp = (long?)value["timestamp"];

            if ((ask == null) || (bid == null) || (symbol == null) || (timestamp == null)) return false;
            return true;
        }

        public override string ToString()
        {
            return "TickRecord{" + "ask=" + ask + ", bid=" + bid + ", askVolume=" + askVolume + ", bidVolume=" + bidVolume + ", high=" + high + ", low=" + low + ", symbol=" + symbol + ", timestamp=" + timestamp + ", level=" + level + ", spreadRaw=" + spreadRaw + ", spreadTable=" + spreadTable + '}';
        }
    }
}