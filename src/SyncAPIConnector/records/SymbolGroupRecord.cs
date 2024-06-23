using System;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    public record SymbolGroupRecord : BaseResponseRecord
    {
        private long? type;
        private string description;
        private string name;

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

        public void FieldsFromJsonObject(JsonObject value)
        {
            this.type = (long?)value["type"];
            this.description = (string)value["description"];
            this.name = (string)value["name"];
        }
    }
}