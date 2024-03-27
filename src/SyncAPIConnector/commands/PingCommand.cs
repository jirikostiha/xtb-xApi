namespace xAPI.Commands
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class PingCommand : BaseCommand
	{
        public PingCommand(bool? prettyPrint)
            : base(new JSONObject(), prettyPrint)
		{
		}

		public override string CommandName
		{
			get
			{
				return "ping";
			}
		}

		public override string[] RequiredArguments
		{
			get
			{
				return new string[]{};
			}
		}
	}
}