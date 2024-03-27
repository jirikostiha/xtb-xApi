namespace xAPI.Commands
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class CalendarCommand : BaseCommand
	{
        public CalendarCommand( bool prettyPrint)
            : base(new JSONObject(), prettyPrint)
		{
		}

		public override string CommandName
		{
			get
			{
                return "getCalendar";
			}
		}

		public override string[] RequiredArguments
		{
			get
			{
				return new string[] {};
			}
		}
	}
}