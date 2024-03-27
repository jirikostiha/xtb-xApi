using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class AllSymbolsResponse : BaseResponse
	{
		private LinkedList<SymbolRecord> symbolRecords = (LinkedList<SymbolRecord>)new LinkedList<SymbolRecord>();

		public AllSymbolsResponse(string body) : base(body)
		{
			JSONArray symbolRecords = (JSONArray) this.ReturnData;
            foreach (JSONObject e in symbolRecords)
            {
                SymbolRecord symbolRecord = new SymbolRecord();
				symbolRecord.FieldsFromJSONObject(e);
                this.symbolRecords.AddLast(symbolRecord);
            }
		}

		public virtual LinkedList<SymbolRecord> SymbolRecords
		{
			get
			{
				return symbolRecords;
			}
		}
	}

}