using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public interface IBaseResponseRecord
{
    void FieldsFromJsonObject(JsonObject value);
}