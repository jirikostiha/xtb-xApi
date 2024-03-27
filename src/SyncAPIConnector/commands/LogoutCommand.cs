namespace xAPI.Commands
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class LogoutCommand : BaseCommand
	{
		public LogoutCommand() : base(new JSONObject(), false)
		{
		}

		public override string ToJSONString()
		{
			JSONObject obj = new JSONObject();
			obj.Add("command", this.commandName);
			return obj.ToString();
		}

		public override string CommandName
		{
			get
			{
				return "logout";
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