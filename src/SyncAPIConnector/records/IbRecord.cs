using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Login}")]
    public record IbRecord : BaseResponseRecord, ISymbol
    {
        /// <summary>
        /// IB close price or null if not allowed to view.
        /// </summary>
        public Double ClosePrice { get; set; }

        /// <summary>
        /// IB user login or null if not allowed to view.
        /// </summary>
        public String Login { get; set; }

        /// <summary>
        /// IB nominal or null if not allowed to view.
        /// </summary>
        public Double Nominal { get; set; }

        /// <summary>
        /// IB open price or null if not allowed to view.
        /// </summary>
        public Double OpenPrice { get; set; }

        /// <summary>
        /// Operation code or null if not allowed to view.
        /// </summary>
        public Side Side { get; set; }

        /// <summary>
        /// IB user surname or null if not allowed to view.
        /// </summary>
        public String Surname { get; set; }

        /// <summary>
        /// Symbol or null if not allowed to view.
        /// </summary>
        public String Symbol { get; set; }

        /// <summary>
        /// Time the record was created or null if not allowed to view.
        /// </summary>
        public Int64 Timestamp { get; set; }

        /// <summary>
        /// Volume in lots or null if not allowed to view.
        /// </summary>
        public Double Volume { get; set; }

        public IbRecord()
        {
        }

        public IbRecord(JsonObject value)
        {
            this.FieldsFromJsonObject(value);
        }

        public void FieldsFromJsonObject(JsonObject value)
        {
            this.ClosePrice = (double)value["closePrice"];
            this.Login = (string)value["login"];
            this.Nominal = (double)value["nominal"];
            this.OpenPrice = (double)value["openPrice"];
            this.Side = Side.FromCode((Int32)value["side"]);
            this.Surname = (string)value["surname"];
            this.Symbol = (string)value["symbol"];
            this.Timestamp = (Int64)value["timestamp"];
            this.Volume = (double)value["volume"];
        }
    }
}
