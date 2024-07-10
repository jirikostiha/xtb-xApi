using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records;

[DebuggerDisplay("day:{Day}, since:{FromT2}, until:{ToT2}")]
public record HoursRecord : IBaseResponseRecord
{
    public long? Day { get; set; }

    /// <summary>
    /// Gets the value of <see cref="Day"/> converted to the corresponding <see cref="DayOfWeek"/>.
    /// Returns <c>null</c> if <see cref="Day"/> is <c>null</c>.
    /// </summary>
    public DayOfWeek? DayOfWeek => Day.HasValue ? ToDayOfWeek(Day.Value) : null;

    public long? FromT { get; set; }

    public long? ToT { get; set; }

    public TimeSpan? FromT2 => FromT is null ? null : TimeSpan.FromMilliseconds(FromT.Value);

    public TimeSpan? ToT2 => ToT is null ? null : TimeSpan.FromMilliseconds(ToT.Value);

    public bool? IsInTimeInterval(TimeSpan timeOfDay)
    {
        if (!FromT.HasValue || !ToT.HasValue)
            return null;

        TimeSpan fromTimeOfDay = TimeSpan.FromMilliseconds(FromT.Value);
        TimeSpan toTimeOfDay = TimeSpan.FromMilliseconds(ToT.Value);

        // Check if the timeOfDay falls between fromTime and toTime
        if (fromTimeOfDay <= toTimeOfDay)
        {
            return timeOfDay >= fromTimeOfDay && timeOfDay <= toTimeOfDay;
        }
        else
        {
            // Crossing midnight
            return timeOfDay >= fromTimeOfDay || timeOfDay <= toTimeOfDay;
        }
    }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Day = (long?)value["day"];
        FromT = (long?)value["fromT"];
        ToT = (long?)value["toT"];
    }

    /// <summary>
    /// Converts a long representing a day (1 to 7) to the corresponding <see cref="DayOfWeek"/>.
    /// </summary>
    /// <param name="day">A long value representing the day, where 1 is Monday and 7 is Sunday.</param>
    /// <returns>
    /// The <see cref="DayOfWeek"/> corresponding to the provided day.
    /// </returns>
    public static DayOfWeek ToDayOfWeek(long day) => day == 7 ? System.DayOfWeek.Sunday : (DayOfWeek)day;
}