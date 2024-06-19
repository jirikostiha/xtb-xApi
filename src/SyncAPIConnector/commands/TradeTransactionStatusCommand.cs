using System.Text.Json.Nodes;

namespace xAPI.Commands
{


    public class TradeTransactionStatusCommand : BaseCommand
    {

        public TradeTransactionStatusCommand(JsonObject arguments, bool prettyPrint)
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