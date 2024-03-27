namespace xAPI.Commands
{

    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class TradeTransactionStatusCommand : BaseCommand
    {

        public TradeTransactionStatusCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "tradeTransactionStatus";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "order" };
            }
        }
    }

}