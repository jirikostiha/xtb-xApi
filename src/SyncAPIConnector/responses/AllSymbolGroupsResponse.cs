using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class AllSymbolGroupsResponse : BaseResponse
	{
		private LinkedList<SymbolGroupRecord> symbolGroupRecords = (LinkedList<SymbolGroupRecord>)new LinkedList<SymbolGroupRecord>();

		public AllSymbolGroupsResponse(string body) : base(body)
		{
			
		}

		public virtual LinkedList<SymbolGroupRecord> SymbolGroupRecords
		{
			get
			{
				return symbolGroupRecords;
			}
		}
	}
}