using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class IbsHistoryCommand : BaseCommand
    {
        public IbsHistoryCommand(JsonObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getIbsHistory";
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