using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class MarginTradeCommand : BaseCommand
    {
        public MarginTradeCommand(JsonObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getMarginTrade";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "symbol", "volume" };
            }
        }
    }
}