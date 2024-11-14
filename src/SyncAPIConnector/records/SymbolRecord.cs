using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using Xtb.XApi.Codes;

namespace Xtb.XApi.Records;

[DebuggerDisplay("{Symbol}, {CategoryName}, {Currency}, {GroupName}")]
public sealed record SymbolRecord : IBaseResponseRecord, ISymbol, ITick
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

    public int? InitialMargin { get; set; }

    public long? InstantMaxVolume { get; set; }

    public double? Leverage { get; set; }

    public bool? LongOnly { get; set; }

    public double? LotMax { get; set; }

    public double? LotMin { get; set; }

    public double? LotStep { get; set; }

    public double? Low { get; set; }

    public int? MarginHedged { get; set; }

    public bool? MarginHedgedStrong { get; set; }

    public int? MarginMaintenance { get; set; }

    public int? Precision { get; set; }

    public double? Percentage { get; set; }

    public int? QuoteId { get; set; }

    public double? SpreadRaw { get; set; }

    public double? SpreadTable { get; set; }

    public int? StepRuleId { get; set; }

    public int? StopsLevel { get; set; }

    public bool? SwapEnable { get; set; }

    public double? SwapLong { get; set; }

    public double? SwapShort { get; set; }

    public string? Symbol { get; set; }

    public double? TickSize { get; set; }

    public double? TickValue { get; set; }

    public int? Type { get; set; } //todo

    public MARGIN_MODE? MarginMode { get; set; }

    public PROFIT_MODE? ProfitMode { get; set; }

    public SWAP_TYPE? SwapType { get; set; }

    public SWAP_ROLLOVER_TYPE? SwapRolloverType { get; set; }

    public DateTimeOffset? StartingTime { get; set; }

    public DateTimeOffset? ExpirationTime { get; set; }

    public DateTimeOffset? Time { get; set; }

    /// <summary>
    /// Indicates if market is cfd stock market.
    /// It is based on symbol name as xtb is using it to distinguish between stocks and cfd stocks.
    /// </summary>
    public bool IsCfdStock => Symbol is not null && Symbol.EndsWith("_4", StringComparison.InvariantCulture);

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
        Precision = (int?)value["precision"];
        StopsLevel = (int?)value["stopsLevel"];
        Symbol = (string?)value["symbol"];
        Type = (int?)value["type"];
        ContractSize = (long?)value["contractSize"];
        InitialMargin = (int?)value["initialMargin"];
        MarginHedged = (int?)value["marginHedged"];
        MarginHedgedStrong = (bool?)value["marginHedgedStrong"];
        MarginMaintenance = (int?)value["marginMaintenance"];
        Percentage = (double?)value["percentage"];
        QuoteId = (int?)value["quoteId"];
        SpreadRaw = (double?)value["spreadRaw"];
        SpreadTable = (double?)value["spreadTable"];
        StepRuleId = (int?)value["stepRuleId"];
        SwapEnable = (bool?)value["swapEnable"];
        SwapLong = (double?)value["swapLong"];
        SwapShort = (double?)value["swapShort"];
        TickSize = (double?)value["tickSize"];
        TickValue = (double?)value["tickValue"];

        var marginModeCode = (int?)value["marginMode"];
        MarginMode = marginModeCode.HasValue ? new MARGIN_MODE(marginModeCode.Value) : null;

        var profitModeCode = (int?)value["profitMode"];
        ProfitMode = profitModeCode.HasValue ? new PROFIT_MODE(profitModeCode.Value) : null;

        var swapType = (int?)value["swapType"];
        SwapType = swapType.HasValue ? new SWAP_TYPE(swapType.Value) : null;

        var swapRolloverCode = (int?)value["swap_rollover3days"];
        SwapRolloverType = swapRolloverCode.HasValue ? new SWAP_ROLLOVER_TYPE(swapRolloverCode.Value) : null;

        var starting = (long?)value["starting"];
        StartingTime = starting.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(starting.Value) : null;
        Debug.Assert(StartingTime?.ToUnixTimeMilliseconds() == starting);

        var time = (long?)value["time"];
        Time = time.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(time.Value) : null;
        Debug.Assert(Time?.ToUnixTimeMilliseconds() == time);

        var expiration = (long?)value["expiration"];
        ExpirationTime = expiration.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(expiration.Value) : null;
        Debug.Assert(ExpirationTime?.ToUnixTimeMilliseconds() == expiration);
    }
}