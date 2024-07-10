using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records;

[DebuggerDisplay("{Symbol}, value:{Value}")]
public record SpreadRecord : IBaseResponseRecord, ISymbol
{
    public long? Precision { get; set; }

    public string? Symbol { get; set; }

    public long? Value { get; set; }

    public long? QuoteId { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Symbol = (string?)value["symbol"];
        Precision = (long?)value["precision"];
        Value = (long?)value["value"];
        QuoteId = (long?)value["quoteId"];
    }
}