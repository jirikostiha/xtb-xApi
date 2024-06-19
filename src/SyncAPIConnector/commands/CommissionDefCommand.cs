using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class CommissionDefCommand : BaseCommand
    {
        public CommissionDefCommand(JsonObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string ToJSONString()
        {
            JsonObject obj = new JsonObject();
            obj.Add("command", commandName);
            obj.Add("prettyPrint", prettyPrint);
            obj.Add("arguments", arguments);
            obj.Add("extended", true);
            return obj.ToString();
        }

        public override string CommandName
        {
            get
            {
                return "getCommissionDef";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "symbol", "volume" };
            }
        }
    }
}