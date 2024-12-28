using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Model;

[DebuggerDisplay("o:{OrderId}, price:{Price}")]
public sealed record StreamingTradeStatusRecord : IBaseResponseRecord, ITradeStatusRecord
{
    public string? CustomComment { get; set; }

    public string? Message { get; set; }

    public long? OrderId { get; set; }

    public double? Price { get; set; }

    public REQUEST_STATUS? RequestStatus { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        CustomComment = (string?)value["customComment"];
        Message = (string?)value["message"];
        OrderId = (long?)value["order"];
        Price = (double?)value["price"];

        var requestStatusCode = (int?)value["requestStatus"];
        RequestStatus = requestStatusCode.HasValue ? new REQUEST_STATUS(requestStatusCode.Value) : null;
    }
}