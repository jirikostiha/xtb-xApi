namespace Xtb.XApiClient.Model;

public sealed class MarginLevelResponse : BaseResponse
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
        MarginLevelRecord = new();
        MarginLevelRecord.FieldsFromJsonObject(ob);
    }

    public MarginLevelRecord? MarginLevelRecord { get; init; }
}