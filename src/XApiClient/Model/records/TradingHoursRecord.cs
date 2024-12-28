using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

[DebuggerDisplay("{Symbol}")]
public sealed record TradingHoursRecord : IBaseResponseRecord, IHasSymbol
{
    public LinkedList<HoursRecord> Quotes { get; set; } = [];

    public LinkedList<HoursRecord> Trading { get; set; } = [];

    public string? Symbol { get; set; }

    /// <summary>
    /// Determines whether the specified time falls within the quote hours.
    /// </summary>
    /// <param name="time">The <see cref="DateTimeOffset"/> to check.</param>
    /// <returns>
    /// <c>true</c> if the specified time is within the quote hours;
    /// <c>false</c> if it is not;
    /// <c>null</c> if the Quotes collection is <c>null</c>.
    /// </returns>
    public bool? IsInQuotesHours(DateTimeOffset time)
    {
        if (Quotes is null)
            return null;

        foreach (var hoursRecord in Quotes)
        {
            if (hoursRecord.DayOfWeek == time.DayOfWeek
                && (hoursRecord.IsInTimeInterval(time.TimeOfDay) ?? false))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified time falls within the trading hours.
    /// </summary>
    /// <param name="time">The <see cref="DateTimeOffset"/> to check.</param>
    /// <returns>
    /// <c>true</c> if the specified time is within the trading hours;
    /// <c>false</c> if it is not;
    /// <c>null</c> if the Trading collection is <c>null</c>.
    /// </returns>
    public bool? IsInTradingHours(DateTimeOffset time)
    {
        if (Trading is null)
            return null;

        foreach (var hoursRecord in Trading)
        {
            if (hoursRecord.DayOfWeek == time.DayOfWeek
                && (hoursRecord.IsInTimeInterval(time.TimeOfDay) ?? false))
                return true;
        }

        return false;
    }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Symbol = (string?)value["symbol"];
        Quotes = new LinkedList<HoursRecord>();
        if (value["quotes"] != null)
        {
            JsonArray jsonarray = value["quotes"]?.AsArray() ?? [];
            foreach (JsonObject jsonObj in jsonarray.OfType<JsonObject>())
            {
                HoursRecord rec = new();
                rec.FieldsFromJsonObject(jsonObj);
                Quotes.AddLast(rec);
            }
        }

        Trading = new LinkedList<HoursRecord>();
        if (value["trading"] != null)
        {
            JsonArray jsonarray = value["trading"]?.AsArray() ?? [];
            foreach (JsonObject jsonObj in jsonarray.OfType<JsonObject>())
            {
                HoursRecord rec = new();
                rec.FieldsFromJsonObject(jsonObj);
                Trading.AddLast(rec);
            }
        }
    }
}