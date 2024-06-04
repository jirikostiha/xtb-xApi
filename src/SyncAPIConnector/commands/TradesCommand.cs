using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class TradesCommand : BaseCommand
    {
        public TradesCommand(JsonObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getTrades";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "openedOnly" };
            }
        }
    }

}