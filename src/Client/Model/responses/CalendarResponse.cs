using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public sealed class CalendarResponse : BaseResponse
{
    public CalendarResponse()
        : base()
    { }

    public CalendarResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        foreach (var e in ReturnData.AsArray().OfType<JsonObject>())
        {
            var record = new CalendarRecord();
            record.FieldsFromJsonObject(e);
            CalendarRecords.Add(record);
        }
    }

    public List<CalendarRecord> CalendarRecords { get; init; } = [];
}