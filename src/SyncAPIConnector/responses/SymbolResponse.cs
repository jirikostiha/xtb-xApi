using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

public class SymbolResponse : BaseResponse
{
    public SymbolResponse()
        : base()
    { }

    public SymbolResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        SymbolRecord = new SymbolRecord();
        SymbolRecord.FieldsFromJsonObject(ob);
    }

    public SymbolRecord? SymbolRecord { get; init; }
}