using System.Text.Json.Nodes;

namespace Xtb.XApi.Model;

public interface IBaseResponseRecord
{
    void FieldsFromJsonObject(JsonObject value);
}