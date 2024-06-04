using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class TickPricesCommand : BaseCommand
    {
        public TickPricesCommand(JsonObject arguments, bool prettyPrint)
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