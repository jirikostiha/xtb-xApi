namespace xAPI.Records
{
    using System;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

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
    }
}