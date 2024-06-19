using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    [DebuggerDisplay("balance:{Balance}, margin:{Margin}, equity:{Equity}")]
    public record StreamingBalanceRecord : BaseResponseRecord
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

        public void FieldsFromJsonObject(JsonObject value)
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

        public void UpdateBy(StreamingBalanceRecord other)
        {
            balance = other.balance;
            margin = other.margin;
            marginFree = other.marginFree;
            marginLevel = other.marginLevel;
            equity = other.equity;
            credit = other.credit;
        }

        public void Reset()
        {
            balance = null;
            margin = null;
            marginFree = null;
            marginLevel = null;
            equity = null;
            credit = null;
        }
    }
}
