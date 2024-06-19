using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class PingCommand : BaseCommand
    {
        public PingCommand(bool? prettyPrint)
            : base(new JsonObject(), prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "ping";
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