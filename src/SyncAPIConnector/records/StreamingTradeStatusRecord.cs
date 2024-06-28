using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records
{
    [DebuggerDisplay("o:{Order}, price:{Price}")]
    public record StreamingTradeStatusRecord : IBaseResponseRecord
    {
        public string? CustomComment { get; set; }

        public string? Message { get; set; }

        public long? Order { get; set; }

        public double? Price { get; set; }

        public REQUEST_STATUS? RequestStatus { get; set; }

        public void FieldsFromJsonObject(JsonObject value)
        {
            this.CustomComment = (string)value["customComment"];
            this.Message = (string)value["message"];
            this.Order = (long?)value["order"];
            this.Price = (double?)value["price"];
            this.RequestStatus = new REQUEST_STATUS((long)value["requestStatus"]);
        }
    }
}
