using System.Linq;
using System;
using System.Text.Json.Nodes;
using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

public sealed class NewsResponse : BaseResponse
{
    public NewsResponse() : base()
    { }

    public NewsResponse(string body) : base(body)
    {
        if (ReturnData is null)
        {
            return;
        }

        var arr = ReturnData.AsArray();
        int count = arr.Count;

        var records = new NewsTopicRecord[count];
        int index = 0;

        foreach (var jsonObj in arr.OfType<JsonObject>())
        {
            var record = new NewsTopicRecord();
            record.FieldsFromJsonObject(jsonObj);
            records[index++] = record;
        }

        NewsTopicRecords = records;
    }

    public NewsTopicRecord[] NewsTopicRecords { get; } = [];
}
