namespace xAPI.Commands
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class TradingHoursCommand : BaseCommand
	{
		public TradingHoursCommand(JSONObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
		{
		}

		public override string CommandName
		{
			get
			{
				return "getTradingHours";
			}
		}

		public override string[] RequiredArguments
		{
			get
			{
				return new string[]{"symbols"};
			}
		}
	}
}