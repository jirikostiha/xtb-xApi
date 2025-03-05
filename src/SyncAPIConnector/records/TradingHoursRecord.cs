using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Records;

[DebuggerDisplay("{Symbol}")]
public sealed record TradingHoursRecord : IBaseResponseRecord, IHasSymbol
{
    public HoursRecord[] Quotes { get; private set; } = [];

    public HoursRecord[] Trading { get; private set; } = [];

    public string? Symbol { get; set; }

    public bool? IsInQuotesHours(DateTimeOffset time)
    {
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

        Quotes = ParseHoursArray(value["quotes"]);
        Trading = ParseHoursArray(value["trading"]);
    }

    private static HoursRecord[] ParseHoursArray(JsonNode? node)
    {
        if (node is not JsonArray jsonArray || jsonArray.Count == 0)
            return Array.Empty<HoursRecord>();

        int count = jsonArray.Count;
        var records = new HoursRecord[count];

        for (int i = 0; i < count; i++)
        {
            if (jsonArray[i] is JsonObject jsonObj)
            {
                var rec = new HoursRecord();
                rec.FieldsFromJsonObject(jsonObj);
                records[i] = rec;
            }
        }

        return records;
    }
}
