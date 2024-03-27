using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using System;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class IbsHistoryResponse : BaseResponse
	{
        /// <summary>
        /// IB records.
        /// </summary>
        public LinkedList<IbRecord> IbRecords { get; set; }

        public IbsHistoryResponse(string body)
            : base(body)
		{
            JSONArray arr = (JSONArray)this.ReturnData;

			foreach (JSONObject e in arr)
			{
                IbRecord record = new IbRecord(e);
                this.IbRecords.AddLast(record);
			}
		}
	}
}