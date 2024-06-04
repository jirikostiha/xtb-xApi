using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Country}, impact:{Impact}")]
    public record CalendarRecord : BaseResponseRecord
    {
        private string country;
        private string current;
        private string forecast;
        private string impact;
        private string period;
        private string previous;
        private long? time;
        private string title;

        public void FieldsFromJsonObject(JsonObject value)
        {
            this.country = (string)value["country"];
            this.current = (string)value["current"];
            this.forecast = (string)value["forecast"];
            this.impact = (string)value["impact"];
            this.period = (string)value["period"];
            this.previous = (string)value["previous"];
            this.time = (long?)value["time"];
            this.title = (string)value["title"];
        }

        public override string ToString()
        {
            return "CalendarRecord[" + "country=" + this.country + ", current=" + this.current + ", forecast=" + this.forecast + ", impact=" + this.impact + ", period=" + this.period + ", previous=" + this.previous + ", time=" + this.time + ", title=" + this.title + "]";
        }

        public string Country
        {
            get { return country; }
        }

        public string Current
        {
            get { return current; }
        }

        public string Forecast
        {
            get { return forecast; }
        }

        public string Impact
        {
            get { return impact; }
        }

        public string Period => period;

        public string Previous
        {
            get { return previous; }
        }

        public long? Time
        {
            get { return time; }
        }

        public string Title
        {
            get { return title; }
        }

        public DateTimeOffset? Time2 => Time is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Time.Value);
    }
}