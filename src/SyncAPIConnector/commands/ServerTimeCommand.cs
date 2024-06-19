using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class ServerTimeCommand : BaseCommand
    {
        public ServerTimeCommand(bool? prettyPrint) : base(new JsonObject(), prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getServerTime";
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