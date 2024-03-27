using xAPI.Records;

namespace xAPI.Responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class SymbolResponse : BaseResponse
	{
		private SymbolRecord symbol;

		public SymbolResponse(string body) : base(body)
		{
			JSONObject ob = (JSONObject) this.ReturnData;
			symbol = new SymbolRecord();
			symbol.FieldsFromJSONObject(ob);
		}

		public virtual SymbolRecord Symbol
		{
			get
			{
				return symbol;
			}
		}
	}
}