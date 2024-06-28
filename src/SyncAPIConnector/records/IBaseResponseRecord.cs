using System.Text.Json.Nodes;

namespace xAPI.Records
{
    public interface IBaseResponseRecord
    {
        void FieldsFromJsonObject(JsonObject value);
    }
}