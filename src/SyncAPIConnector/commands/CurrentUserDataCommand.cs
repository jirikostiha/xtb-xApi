using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class CurrentUserDataCommand : BaseCommand
    {
        public CurrentUserDataCommand(bool prettyPrint) : base(new JsonObject(), prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getCurrentUserData";
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