namespace xAPI.Records
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public interface BaseResponseRecord
	{
		void FieldsFromJSONObject(JSONObject value);
	}
}