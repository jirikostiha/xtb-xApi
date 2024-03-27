namespace xAPI.Commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class TradeRecordsCommand : BaseCommand
    {
        public TradeRecordsCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getTradeRecords";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "orders" };
            }
        }
    }

}