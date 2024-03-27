namespace xAPI.Records
{
    using System;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class SymbolGroupRecord : BaseResponseRecord
	{
		private long? type;
		private string description;
		private string name;

        [Obsolete("Command getAllSymbolGroups is not available in API any more")]
		public SymbolGroupRecord()
		{
		}

		public virtual long? Type
		{
			get
			{
				return type;
			}
		}

		public virtual string Description
		{
			get
			{
				return description;
			}
		}

		public virtual string Name
		{
			get
			{
				return name;
			}
		}

        public void FieldsFromJSONObject(JSONObject value)
        {
            this.type = (long?)value["type"];
            this.description = (string)value["description"];
            this.name = (string)value["name"];
        }
    }
}