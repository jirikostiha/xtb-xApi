using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public sealed class CurrentUserDataRecord : IBaseResponseRecord
{
    public string? Currency { get; set; }

    public double? LeverageMultiplier { get; set; }

    public string? Group { get; set; }

    public int? CompanyUnit { get; set; }

    public string? SpreadType { get; set; }

    public bool? IbAccount { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Currency = (string?)value["currency"];
        LeverageMultiplier = (double?)value["leverageMultiplier"];
        Group = (string?)value["group"];
        CompanyUnit = (int?)value["companyUnit"];
        SpreadType = (string?)value["spreadType"];
        IbAccount = (bool?)value["ibAccount"];
    }
}