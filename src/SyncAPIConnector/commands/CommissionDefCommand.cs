namespace xAPI.Commands
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class CommissionDefCommand : BaseCommand
	{
		public CommissionDefCommand(JSONObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
		{
		}

		public override string ToJSONString()
		{
			JSONObject obj = new JSONObject();
			obj.Add("command", commandName);
			obj.Add("prettyPrint", prettyPrint);
			obj.Add("arguments", arguments);
			obj.Add("extended", true);
			return obj.ToString();
		}

		public override string CommandName
		{
			get
			{
				return "getCommissionDef";
			}
		}

		public override string[] RequiredArguments
		{
			get
			{
				return new string[]{"symbol", "volume"};
			}
		}
	}
}