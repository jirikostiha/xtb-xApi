using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records;

[DebuggerDisplay("{Symbol}, {CategoryName}, {Currency}, {GroupName}")]
public record SymbolRecord : IBaseResponseRecord, ISymbol, ITick
{
    public double? Ask { get; set; }

    public double? Bid { get; set; }

    public string? CategoryName { get; set; }

    public long? ContractSize { get; set; }

    public string? Currency { get; set; }

    public bool? CurrencyPair { get; set; }

    public string? CurrencyProfit { get; set; }

    public string? Description { get; set; }

    public string? GroupName { get; set; }

    public double? High { get; set; }

    public long? InitialMargin { get; set; }

    public long? InstantMaxVolume { get; set; }

    public double? Leverage { get; set; }

    public bool? LongOnly { get; set; }

    public double? LotMax { get; set; }

    public double? LotMin { get; set; }

    public double? LotStep { get; set; }

    public double? Low { get; set; }

    public long? MarginHedged { get; set; }

    public bool? MarginHedgedStrong { get; set; }

    public long? MarginMaintenance { get; set; }

    public long? Precision { get; set; }

    public double? Percentage { get; set; }

    public long? QuoteId { get; set; }

    public double? SpreadRaw { get; set; }

    public double? SpreadTable { get; set; }

    public long? Starting { get; set; }

    public long? StepRuleId { get; set; }

    public long? StopsLevel { get; set; }

    public bool? SwapEnable { get; set; }

    public double? SwapLong { get; set; }

    public double? SwapShort { get; set; }

    public string? Symbol { get; set; }

    public double? TickSize { get; set; }

    public double? TickValue { get; set; }

    public long? Type { get; set; }

    public MARGIN_MODE? MarginMode { get; set; }

    public PROFIT_MODE? ProfitMode { get; set; }

    public SWAP_TYPE? SwapType { get; set; }

    public SWAP_ROLLOVER_TYPE? SwapRolloverType { get; set; }

    public DateTimeOffset? ExpirationTime { get; set; }

    public DateTimeOffset? Time { get; set; }

    /// <summary>
    /// Indicates if market is cfd stock market.
    /// It is based on symbol name as xtb is using it to distinguish between stocks and cfd stocks.
    /// </summary>
    public virtual bool IsCfdStock => Symbol is not null && Symbol.EndsWith("_4", StringComparison.InvariantCulture);

    public void FieldsFromJsonObject(JsonObject value)
    {
        Ask = (double?)value["ask"];
        Bid = (double?)value["bid"];
        CategoryName = (string?)value["categoryName"];
        Currency = (string?)value["currency"];
        CurrencyPair = (bool?)value["currencyPair"];
        CurrencyProfit = (string?)value["currencyProfit"];
        Description = (string?)value["description"];
        GroupName = (string?)value["groupName"];
        High = (double?)value["high"];
        InstantMaxVolume = (long?)value["instantMaxVolume"];
        Leverage = (double?)value["leverage"];
        LongOnly = (bool?)value["longOnly"];
        LotMax = (double?)value["lotMax"];
        LotMin = (double?)value["lotMin"];
        LotStep = (double?)value["lotStep"];
        Low = (double?)value["low"];
        Precision = (long?)value["precision"];
        Starting = (long?)value["starting"];
        StopsLevel = (long?)value["stopsLevel"];
        Symbol = (string?)value["symbol"];
        Type = (long?)value["type"];
        ContractSize = (long?)value["contractSize"];
        InitialMargin = (long?)value["initialMargin"];
        MarginHedged = (long?)value["marginHedged"];
        MarginHedgedStrong = (bool?)value["marginHedgedStrong"];
        MarginMaintenance = (long?)value["marginMaintenance"];
        Percentage = (double?)value["percentage"];
        QuoteId = (long?)value["quoteId"];
        SpreadRaw = (double?)value["spreadRaw"];
        SpreadTable = (double?)value["spreadTable"];
        StepRuleId = (long?)value["stepRuleId"];
        SwapEnable = (bool?)value["swapEnable"];
        SwapLong = (double?)value["swapLong"];
        SwapShort = (double?)value["swapShort"];
        TickSize = (double?)value["tickSize"];
        TickValue = (double?)value["tickValue"];

        var marginModeCode = (long?)value["marginMode"];
        MarginMode = marginModeCode.HasValue ? new MARGIN_MODE(marginModeCode.Value) : null;

        var profitModeCode = (long?)value["profitMode"];
        ProfitMode = profitModeCode.HasValue ? new PROFIT_MODE(profitModeCode.Value) : null;

        var swapType = (long?)value["swapType"];
        SwapType = swapType.HasValue ? new SWAP_TYPE(swapType.Value) : null;

        var swapRolloverCode = (long?)value["swap_rollover3days"];
        SwapRolloverType = swapRolloverCode.HasValue ? new SWAP_ROLLOVER_TYPE(swapRolloverCode.Value) : null;

        var time = (long?)value["time"];
        Time = time.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(time.Value) : null;

        var expiration = (long?)value["expiration"];
        ExpirationTime = expiration.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(expiration.Value) : null;
    }
}