namespace xAPI.Records
{
    using System;
    using System.Diagnostics;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    [DebuggerDisplay("day:{Day}, since:{FromT2}, until:{ToT2}")]
    public record HoursRecord : BaseResponseRecord
    {
        private long? day;
        private long? fromT;
        private long? toT;

        public virtual long? Day
        {
            get
            {
                return day;
            }
        }

        /// <summary>
        /// Gets the value of <see cref="Day"/> converted to the corresponding <see cref="DayOfWeek"/>.
        /// Returns <c>null</c> if <see cref="Day"/> is <c>null</c>.
        /// </summary>
        public DayOfWeek? DayOfWeek => day.HasValue ? ToDayOfWeek(day.Value) : null;

        public virtual long? FromT
        {
            get
            {
                return fromT;
            }
        }

        public virtual long? ToT
        {
            get
            {
                return toT;
            }
        }

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

        public void FieldsFromJSONObject(JSONObject value)
        {
            this.day = (long?)value["day"];
            this.fromT = (long?)value["fromT"];
            this.toT = (long?)value["toT"];
        }

        public override string ToString()
        {
            return "HoursRecord{" + "day=" + this.day + ", fromT=" + this.fromT + ", toT=" + this.toT + '}';
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
}