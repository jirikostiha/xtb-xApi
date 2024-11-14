namespace Xtb.XApi.Responses;

public sealed class CurrentUserDataResponse : BaseResponse
{
    public CurrentUserDataResponse()
        : base()
    { }

    public CurrentUserDataResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        Currency = (string?)ob["currency"];
        LeverageMultiplier = (double?)ob["leverageMultiplier"];
        Group = (string?)ob["group"];
        CompanyUnit = (int?)ob["companyUnit"];
        SpreadType = (string?)ob["spreadType"];
        IbAccount = (bool?)ob["ibAccount"];
    }

    public string? Currency { get; init; }

    public double? LeverageMultiplier { get; init; }

    public string? Group { get; init; }

    public int? CompanyUnit { get; init; }

    public string? SpreadType { get; init; }

    public bool? IbAccount { get; init; }
}