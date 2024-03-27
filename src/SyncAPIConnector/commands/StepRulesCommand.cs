namespace xAPI.Commands
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class StepRulesCommand : BaseCommand
	{
        public StepRulesCommand()
            : base(new JSONObject(), false)
		{
		}

		public override string CommandName
		{
			get
			{
                return "getStepRules";
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