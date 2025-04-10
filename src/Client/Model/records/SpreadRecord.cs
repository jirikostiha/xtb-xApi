﻿using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

[DebuggerDisplay("{Symbol}, value:{Value}")]
public sealed record SpreadRecord : IBaseResponseRecord, IHasSymbol
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