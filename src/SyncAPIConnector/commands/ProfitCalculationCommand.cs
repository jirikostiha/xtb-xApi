namespace xAPI.Commands
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class ProfitCalculationCommand : BaseCommand
	{

		public ProfitCalculationCommand(JSONObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
		{
		}

		public override string CommandName
		{
			get
			{
                return "getProfitCalculation";
			}
		}

		public override string[] RequiredArguments
		{
			get
			{
                return new string[] { "cmd", "symbol", "volume", "openPrice", "closePrice" };
			}
		}
	}
}