namespace xAPI.Commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class TradesHistoryCommand : BaseCommand
    {
        public TradesHistoryCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getTradesHistory";
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