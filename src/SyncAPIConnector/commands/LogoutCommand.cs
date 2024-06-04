using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class LogoutCommand : BaseCommand
    {
        public LogoutCommand() : base(new JsonObject(), false)
        {
        }

        public override string ToJSONString()
        {
            JsonObject obj = new JsonObject();
            obj.Add("command", this.commandName);
            return obj.ToString();
        }

        public override string CommandName
        {
            get
            {
                return "logout";
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