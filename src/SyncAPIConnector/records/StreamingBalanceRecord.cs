using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    [DebuggerDisplay("balance:{Balance}, margin:{Margin}, equity:{Equity}")]
    public record StreamingBalanceRecord : IBaseResponseRecord
    {
        public double? Balance { get; set; }

        public double? Margin { get; set; }

        public double? MarginFree { get; set; }

        public double? MarginLevel { get; set; }

        public double? Equity { get; set; }

        public double? Credit { get; set; }

        public void FieldsFromJsonObject(JsonObject value)
        {
            Balance = (double?)value["balance"];
            Margin = (double?)value["margin"];
            MarginFree = (double?)value["marginFree"];
            MarginLevel = (double?)value["marginLevel"];
            Equity = (double?)value["equity"];
            Credit = (double?)value["credit"];
        }

        public void UpdateBy(StreamingBalanceRecord other)
        {
            Balance = other.Balance;
            Margin = other.Margin;
            MarginFree = other.MarginFree;
            MarginLevel = other.MarginLevel;
            Equity = other.Equity;
            Credit = other.Credit;
        }

        public void Reset()
        {
            Balance = null;
            Margin = null;
            MarginFree = null;
            MarginLevel = null;
            Equity = null;
            Credit = null;
        }
    }
}
