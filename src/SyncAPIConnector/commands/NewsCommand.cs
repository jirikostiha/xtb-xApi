using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class NewsCommand : BaseCommand
    {
        public NewsCommand(JsonObject body, bool prettyPrint)
            : base(body, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getNews";
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