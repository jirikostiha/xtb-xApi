namespace xAPI.Commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class TickPricesCommand : BaseCommand
    {
        public TickPricesCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getTickPrices";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "symbols", "timestamp" };
            }
        }
    }
}