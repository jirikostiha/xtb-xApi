namespace xAPI.Commands
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class IbsHistoryCommand : BaseCommand
	{
        public IbsHistoryCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
		{
		}

		public override string CommandName
		{
			get
			{
                return "getIbsHistory";
			}
		}

		public override string[] RequiredArguments
		{
			get
			{
                return new string[] { "start", "end" };
			}
		}
	}
}