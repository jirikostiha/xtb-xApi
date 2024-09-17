using System.Diagnostics;
using System.Text.Json.Nodes;

namespace XApi.Records;

[DebuggerDisplay("o:{Order}, o2:{Order2}, profit:{Profit}")]
public record StreamingProfitRecord : IBaseResponseRecord, IPosition
{
    public long? Order { get; set; }

    public long? Order2 { get; set; }

    public long? Position { get; set; }

    public double? Profit { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Profit = (double?)value["profit"];
        Order = (long?)value["order"];
        Order2 = (long?)value["order2"]; //todo check
        Position = (long?)value["position"]; //todo check
    }

    public void UpdateBy(StreamingProfitRecord other)
    {
        Order = other.Order;
        Order2 = other.Order2;
        Position = other.Position;
        Profit = other.Profit;
    }

    public void Reset()
    {
        Order = null;
        Order2 = null;
        Position = null;
        Profit = null;
    }
}