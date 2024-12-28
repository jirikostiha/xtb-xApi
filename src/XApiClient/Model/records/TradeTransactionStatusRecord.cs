using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

[DebuggerDisplay("orderId:{OrderId}")]
public sealed class TradeTransactionStatusRecord : ITradeStatusRecord
{
    public long? OrderId { get; set; }

    public double? Ask { get; set; }

    public double? Bid { get; set; }

    public string? CustomComment { get; set; }

    public string? Message { get; set; }

    public REQUEST_STATUS? RequestStatus { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        OrderId = (long?)value["order"];
        Ask = (double?)value["ask"];
        Bid = (double?)value["bid"];
        CustomComment = (string?)value["customComment"];
        Message = (string?)value["message"];

        var requestStatusCode = (int?)value["requestStatus"];
        RequestStatus = requestStatusCode.HasValue ? new REQUEST_STATUS(requestStatusCode.Value) : null;
    }
}