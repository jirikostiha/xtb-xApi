namespace XApi.Responses;

public class ProfitCalculationResponse : BaseResponse
{
    public ProfitCalculationResponse()
        : base()
    { }

    public ProfitCalculationResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        Profit = (double?)ob["profit"];
    }

    public double? Profit { get; init; }
}