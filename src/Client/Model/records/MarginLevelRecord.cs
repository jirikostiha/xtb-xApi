using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public sealed class MarginLevelRecord : IBaseResponseRecord, IBalanceRecord
{
    public double? Balance { get; set; }

    public double? Equity { get; set; }

    public string? Currency { get; set; }

    public double? Margin { get; set; }

    public double? MarginFree { get; set; }

    public double? MarginLevel { get; set; }

    public double? Credit { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Balance = (double?)value["balance"];
        Equity = (double?)value["equity"];
        Currency = (string?)value["currency"];
        Margin = (double?)value["margin"];
        MarginFree = (double?)value["margin_free"];
        MarginLevel = (double?)value["margin_level"];
        Credit = (double?)value["credit"];
    }
}