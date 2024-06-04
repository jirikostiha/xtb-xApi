using System.Collections.Generic;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{

    public class NewsResponse : BaseResponse
    {
        private LinkedList<NewsTopicRecord> newsTopicRecords = (LinkedList<NewsTopicRecord>)new LinkedList<NewsTopicRecord>();

        public NewsResponse(string body) : base(body)
        {
            JsonArray arr = this.ReturnData.AsArray();
            foreach (JsonObject e in arr)
            {
                NewsTopicRecord record = new NewsTopicRecord();
                record.FieldsFromJsonObject(e);
                newsTopicRecords.AddLast(record);
            }
        }

        public virtual LinkedList<NewsTopicRecord> NewsTopicRecords
        {
            get
            {
                return newsTopicRecords;
            }
        }
    }
}