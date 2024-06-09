using System.Collections.Generic;
using System.Diagnostics;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    [DebuggerDisplay("o:{Order}, o2:{Order2}, profit:{Profit}")]
    public record StreamingProfitRecord : BaseResponseRecord, IPosition
    {
        private long? order;
        private long? order2;
        private long? position;
        private double? profit;

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

        public void FieldsFromJSONObject(JSONObject value)
        {
            this.profit = (double?)value["profit"];
            this.order = (long?)value["order"];
        }

        public override string ToString()
        {
            return "StreamingProfitRecord{" +
                "profit=" + profit +
                ", order=" + order +
                '}';
        }

        public void UpdateBy(StreamingProfitRecord other)
        {
            order = other.order;
            order2 = other.order2;
            position = other.position;
            profit = other.profit;
        }

        public void Reset()
        {
            order = null;
            order2 = null;
            position = null;
            profit = null;
        }
    }
}
