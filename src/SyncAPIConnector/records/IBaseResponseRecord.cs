using System.Text.Json.Nodes;

namespace Xtb.XApi.Records;

public interface IBaseResponseRecord
{
    void FieldsFromJsonObject(JsonObject value);
}