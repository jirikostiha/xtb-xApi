using System.Collections.Generic;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class StreamingBalanceRecord : BaseResponseRecord
    {
        private double? balance;
        private double? margin;
        private double? marginFree;
        private double? marginLevel;
        private double? equity;
        private double? credit;

        public double? Balance
        {
            get { return balance; }
            set { balance = value; }
        }
        public double? Margin
        {
            get { return margin; }
            set { margin = value; }
        }
        public double? MarginFree
        {
            get { return marginFree; }
            set { marginFree = value; }
        }
        public double? MarginLevel
        {
            get { return marginLevel; }
            set { marginLevel = value; }
        }
        public double? Equity
        {
            get { return equity; }
            set { equity = value; }
        }
        public double? Credit
        {
            get { return credit; }
            set { credit = value; }
        }

        public void FieldsFromJSONObject(JSONObject value)
        {
            Balance = (double?)value["balance"];
            Margin = (double?)value["margin"];
            MarginFree = (double?)value["marginFree"];
            MarginLevel = (double?)value["marginLevel"];
            Equity = (double?)value["equity"];
            Credit = (double?)value["credit"];
        }

        public override string ToString()
        {
            return "StreamingBalanceRecord{" +
                "balance=" + Balance +
                ", margin=" + Margin +
                ", marginFree=" + MarginFree +
                ", marginLevel=" + MarginLevel +
                ", equity=" + Equity +
                 ", credit=" + Credit +
                '}';
        }
    }
}
