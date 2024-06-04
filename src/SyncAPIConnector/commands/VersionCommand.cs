using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class VersionCommand : BaseCommand
    {
        public VersionCommand(JsonObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getVersion";
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