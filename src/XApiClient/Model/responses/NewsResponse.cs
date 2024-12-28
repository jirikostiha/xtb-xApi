using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

public sealed class NewsResponse : BaseResponse
{
    public NewsResponse()
        : base()
    { }

    public NewsResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var arr = ReturnData.AsArray();
        foreach (var e in arr.OfType<JsonObject>())
        {
            var record = new NewsTopicRecord();
            record.FieldsFromJsonObject(e);
            NewsTopicRecords.AddLast(record);
        }
    }

    public LinkedList<NewsTopicRecord> NewsTopicRecords { get; init; } = [];
}