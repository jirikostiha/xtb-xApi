using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records;

[DebuggerDisplay("{Symbol}, value:{Value}")]
public record SpreadRecord : IBaseResponseRecord, ISymbol
{
    public int? Precision { get; set; }

    public string? Symbol { get; set; }

    public long? Value { get; set; }

    public int? QuoteId { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Symbol = (string?)value["symbol"];
        Precision = (int?)value["precision"];
        Value = (long?)value["value"];
        QuoteId = (int?)value["quoteId"];
    }
}