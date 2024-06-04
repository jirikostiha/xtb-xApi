using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class MarginLevelCommand : BaseCommand
    {
        public MarginLevelCommand(bool? prettyPrint) : base(new JsonObject(), prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getMarginLevel";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return [];
            }
        }
    }
}