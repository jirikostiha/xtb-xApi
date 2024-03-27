using System.Collections.Generic;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class StreamingProfitRecord : BaseResponseRecord
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
    }
}
