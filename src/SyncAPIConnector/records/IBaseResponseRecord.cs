using System.Text.Json.Nodes;

namespace XApi.Records;

public interface IBaseResponseRecord
{
    void FieldsFromJsonObject(JsonObject value);
}