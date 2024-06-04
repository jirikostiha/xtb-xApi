using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class SymbolCommand : BaseCommand
    {
        public SymbolCommand(JsonObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getSymbol";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "symbol" };
            }
        }
    }
}