using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

[DebuggerDisplay("day:{DayOfWeek}, since:{FromTime}, until:{ToTime}")]
public sealed record HoursRecord : IBaseResponseRecord
{
    public DayOfWeek? DayOfWeek { get; set; }

    public TimeSpan? FromTime { get; set; }

    public TimeSpan? ToTime { get; set; }

    public bool? IsInTimeInterval(TimeSpan timeOfDay)
    {
        if (!FromTime.HasValue || !ToTime.HasValue)
            return null;

        // Check if the timeOfDay falls between fromTime and toTime
        if (FromTime <= ToTime)
        {
            return timeOfDay >= FromTime && timeOfDay <= ToTime;
        }
        else
        {
            // Crossing midnight
            return timeOfDay >= FromTime || timeOfDay <= ToTime;
        }
    }

    public void FieldsFromJsonObject(JsonObject value)
    {
        var day = (int?)value["day"];
        DayOfWeek = day.HasValue ? ToDayOfWeek(day.Value) : null;

        var fromTime = (long?)value["fromT"];
        FromTime = fromTime.HasValue ? TimeSpan.FromMilliseconds(fromTime.Value) : null;

        var toTime = (long?)value["toT"];
        ToTime = toTime.HasValue ? TimeSpan.FromMilliseconds(toTime.Value) : null;
    }

    /// <summary>
    /// Converts a long representing a day (1 to 7) to the corresponding <see cref="DayOfWeek"/>.
    /// </summary>
    /// <param name="day">A long value representing the day, where 1 is Monday and 7 is Sunday.</param>
    /// <returns>
    /// The <see cref="DayOfWeek"/> corresponding to the provided day.
    /// </returns>
    public static DayOfWeek ToDayOfWeek(int day) => day == 7 ? System.DayOfWeek.Sunday : (DayOfWeek)day;
}