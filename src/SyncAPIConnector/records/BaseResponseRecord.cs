using System.Text.Json.Nodes;

namespace xAPI.Records
{

    public interface BaseResponseRecord
    {
        void FieldsFromJsonObject(JsonObject value);
    }
}