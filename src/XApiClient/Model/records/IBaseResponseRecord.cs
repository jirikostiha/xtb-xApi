using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

public interface IBaseResponseRecord
{
    void FieldsFromJsonObject(JsonObject value);
}