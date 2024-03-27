namespace xAPI.Commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class TradeTransactionCommand : BaseCommand
    {
        public TradeTransactionCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "tradeTransaction";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "tradeTransInfo" };
            }
        }
    }

}