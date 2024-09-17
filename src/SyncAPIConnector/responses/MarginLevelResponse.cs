namespace XApi.Responses;

public class MarginLevelResponse : BaseResponse
{
    public MarginLevelResponse()
        : base()
    { }

    public MarginLevelResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        Balance = (double?)ob["balance"];
        Equity = (double?)ob["equity"];
        Currency = (string?)ob["currency"];
        Margin = (double?)ob["margin"];
        MarginFree = (double?)ob["margin_free"];
        MarginLevel = (double?)ob["margin_level"];
        Credit = (double?)ob["credit"];
    }

    public double? Balance { get; init; }

    public double? Equity { get; init; }

    public string? Currency { get; init; }

    public double? Margin { get; init; }

    public double? MarginFree { get; init; }

    public double? MarginLevel { get; init; }

    public double? Credit { get; init; }
}