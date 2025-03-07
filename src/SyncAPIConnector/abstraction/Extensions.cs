using System;
using System.Collections.Generic;
using System.Linq;
using Xtb.XApi.Codes;
using Xtb.XApi.Records;

namespace Xtb.XApi;

public static class TradeRecordExtensions
{
    /// <summary> Middle price of ask and bid in base currency. </summary>
    public static double? Mid(this ITick tick) => (tick.Bid - tick.Ask) / 2d;

    /// <summary> Size of the candle. </summary>
    public static double? Size(this ICandle candle) => candle.High - candle.Low;

    /// <summary> Size of the candle as coefficient with open price as base. </summary>
    public static double? SizeCoef(this ICandle candle) => (candle.High - candle.Low) / candle.Open;

    /// <summary> Average price of the candle. </summary>
    public static double? Average(this ICandle candle) => (candle.High + candle.Low) / 2d;

    /// <summary>
    /// Determines whether the specified trade is a long position.
    /// </summary>
    /// <param name="trade">The trade record.</param>
    /// <returns>
    /// <c>true</c> if the trade is a long position (BUY, BUY_LIMIT, or BUY_STOP); otherwise, <c>false</c>.
    /// </returns>
    public static bool IsLongPosition(this ITradeRecord trade) =>
        trade.TradeOperation is not null
        && (trade.TradeOperation == TRADE_OPERATION_TYPE.BUY || trade.TradeOperation == TRADE_OPERATION_TYPE.BUY_LIMIT || trade.TradeOperation == TRADE_OPERATION_TYPE.BUY_STOP);

    /// <summary>
    /// Determines whether the specified trade is a short position.
    /// </summary>
    /// <param name="trade">The trade record.</param>
    /// <returns>
    /// <c>true</c> if the trade is a short position (SELL, SELL_LIMIT, or SELL_STOP); otherwise, <c>false</c>.
    /// </returns>
    public static bool IsShortPosition(this ITradeRecord trade) =>
        trade.TradeOperation is not null
        && (trade.TradeOperation == TRADE_OPERATION_TYPE.SELL || trade.TradeOperation == TRADE_OPERATION_TYPE.SELL_LIMIT || trade.TradeOperation == TRADE_OPERATION_TYPE.SELL_STOP);

    /// <summary>
    /// Net profit in account currency. It is gross profit minus fees for storage (swap + rollover)
    /// </summary>
    public static double? NetProfit(this ITradeRecord trade) =>
        trade.Profit + trade.Storage;

    /// <summary>
    /// Rounds the profit of a trade to the specified number of digits.
    /// </summary>
    public static double RoundedProfit(this ITradeRecord trade, int defaultDigits = 5) =>
        Math.Round(trade.Profit ?? 0d, trade.Digits ?? defaultDigits);

    /// <summary>
    /// Rounds the total profit of multiple trades to the specified number of digits.
    /// </summary>
    public static double RoundedProfit(this IEnumerable<ITradeRecord> trades, int digits = 5) =>
        Math.Round(trades.Sum(x => x?.Profit ?? 0d), digits);

    /// <summary>
    /// Rounds the profit of a trade converted using an exchange rate.
    /// </summary>
    public static double RoundedProfit(this ITradeRecord trade, double exchangeRate, int defaultDigits = 5) =>
        Math.Round((trade.Profit ?? 0d) * exchangeRate, trade.Digits ?? defaultDigits);

    /// <summary>
    /// Rounds the total profit of multiple trades converted using an exchange rate.
    /// </summary>
    public static double RoundedProfit(this IEnumerable<ITradeRecord> trades, double exchangeRate, int digits = 5) =>
        Math.Round(trades.Sum(x => x.Profit ?? 0d) * exchangeRate, digits);

    /// <summary>
    /// Rounds the storage fee of a trade to the specified number of digits.
    /// </summary>
    public static double RoundedStorage(this ITradeRecord trade, int defaultDigits = 5) =>
        Math.Round(trade.Storage ?? 0d, trade.Digits ?? defaultDigits);

    /// <summary>
    /// Rounds the total storage fees of multiple trades to the specified number of digits.
    /// </summary>
    public static double RoundedStorage(this IEnumerable<ITradeRecord> trades, int digits = 5) =>
        Math.Round(trades.Sum(x => x.Storage) ?? 0d, digits);

    /// <summary>
    /// Rounds the storage fee of a trade converted using an exchange rate.
    /// </summary>
    public static double RoundedStorage(this ITradeRecord trade, double exchangeRate, int defaultDigits = 5) =>
        Math.Round((trade.Storage ?? 0d) * exchangeRate, trade.Digits ?? defaultDigits);

    /// <summary>
    /// Rounds the total storage fees of multiple trades converted using an exchange rate.
    /// </summary>
    public static double RoundedStorage(this IEnumerable<ITradeRecord> trades, double exchangeRate, int digits = 5) =>
        Math.Round(trades.Sum(x => x.Storage ?? 0d) * exchangeRate, digits);

    /// <summary>
    /// Rounds the net profit of a trade to the specified number of digits.
    /// </summary>
    public static double RoundedNetProfit(this ITradeRecord trade, int defaultDigits = 5) =>
        Math.Round(trade?.NetProfit() ?? 0d, trade?.Digits ?? defaultDigits);

    /// <summary>
    /// Rounds the total net profit of multiple trades to the specified number of digits.
    /// </summary>
    public static double RoundedNetProfit(this IEnumerable<ITradeRecord> trades, int digits = 5) =>
        Math.Round(trades.Sum(x => x.NetProfit()) ?? 0d, digits);

    /// <summary>
    /// Rounds the net profit of a trade converted using an exchange rate.
    /// </summary>
    public static double RoundedNetProfit(this ITradeRecord trade, double exchangeRate, int defaultDigits = 5) =>
        Math.Round((trade.NetProfit() ?? 0d) * exchangeRate, trade.Digits ?? defaultDigits);

    /// <summary>
    /// Rounds the total net profit of multiple trades converted using an exchange rate.
    /// </summary>
    public static double RoundedNetProfit(this IEnumerable<ITradeRecord> trades, double exchangeRate, int digits = 5) =>
        Math.Round(trades.Sum(x => x.NetProfit() ?? 0d) * exchangeRate, digits);

    /// <summary>
    /// Calculates the average open price of multiple trades.
    /// Warning: Trades must be of the same instrument for meaningful results.
    /// </summary>
    public static double? AverageOpenPrice(this IEnumerable<ITradeRecord> trades)
    {
        var totalPrice = trades.Sum(t => t.OpenPrice);
        return totalPrice / trades.Count();
    }

    /// <summary>
    /// Calculates the average gross profit of multiple trades.
    /// Warning: Trades must be of the same instrument for meaningful results.
    /// </summary>
    public static double? AverageGrossProfit(this IEnumerable<ITradeRecord> trades)
    {
        var totalProfit = trades.Sum(t => t.Profit);
        return totalProfit / trades.Count();
    }

    /// <summary>
    /// Calculates the average storage fee of multiple trades.
    /// Warning: Trades must be of the same instrument for meaningful results.
    /// </summary>
    public static double? AverageStorage(this IEnumerable<ITradeRecord> trades)
    {
        var totalFees = trades.Sum(t => t.Storage);
        return totalFees / trades.Count();
    }

    /// <summary>
    /// Calculates the average net profit of multiple trades.
    /// Warning: Trades must be of the same instrument for meaningful results.
    /// </summary>
    public static double? AverageNetProfit(this IEnumerable<ITradeRecord> trades)
    {
        var totalProfit = trades.Sum(t => t.NetProfit());
        return totalProfit / trades.Count();
    }

    /// <summary>
    /// Calculates the net zero profit price based on the average open price and storage fees.
    /// </summary>
    public static double? NetZeroProfitPrice(this IEnumerable<ITradeRecord> trades) =>
        AverageOpenPrice(trades) - AverageStorage(trades);

    /// <summary>
    /// Calculates the absolute total traded volume.
    /// </summary>
    public static double? AbsoluteVolume(this IEnumerable<ITradeRecord> trades) =>
        trades.Sum(t => Math.Abs(t.Volume ?? 0d));

    /// <summary>
    /// Calculates the relative total traded volume: long volume - short volume.
    /// </summary>
    public static double? RelativeVolume(this IEnumerable<ITradeRecord> trades) =>
        trades.Sum(t => t.Volume ?? 0d);

    /// <summary>
    /// Determines whether the specified time falls within the hours.
    /// </summary>
    /// <param name="hours"> Collection of hours. </param>
    /// <param name="time">The <see cref="DateTime"/> to check.</param>
    /// <returns>
    /// <c>true</c> if the specified time is within the trading hours;
    /// <c>false</c> if it is not;
    /// </returns>
    public static bool IsInsideHours(this IEnumerable<HoursRecord> hours, DateTime time)
    {
        foreach (var hour in hours)
        {
            if (hour.DayOfWeek == time.DayOfWeek
                && (hour.IsInTimeInterval(time.TimeOfDay) ?? false))
                return true;
        }
        return false;
    }
}