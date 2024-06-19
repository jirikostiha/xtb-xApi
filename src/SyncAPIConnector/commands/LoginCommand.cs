using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class LoginCommand : BaseCommand
    {
        public LoginCommand(JsonObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "login";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "userId", "password" };
            }
        }
    }
}