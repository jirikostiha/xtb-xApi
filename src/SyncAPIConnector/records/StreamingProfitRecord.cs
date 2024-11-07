using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Records;

[DebuggerDisplay("o:{OrderId}, o2:{Order2Id}, profit:{Profit}")]
public record StreamingProfitRecord : IBaseResponseRecord, IPosition
{
    public long? OrderId { get; set; }

    public long? Order2Id { get; set; }

    public long? PositionId { get; set; }

    public double? Profit { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Profit = (double?)value["profit"];
        OrderId = (long?)value["order"];
        Order2Id = (long?)value["order2"]; //todo check
        PositionId = (long?)value["position"]; //todo check
    }

    public void UpdateBy(StreamingProfitRecord other)
    {
        OrderId = other.OrderId;
        Order2Id = other.Order2Id;
        PositionId = other.PositionId;
        Profit = other.Profit;
    }

    public void Reset()
    {
        OrderId = null;
        Order2Id = null;
        PositionId = null;
        Profit = null;
    }
}