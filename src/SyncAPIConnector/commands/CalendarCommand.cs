using System.Text.Json.Nodes;

namespace xAPI.Commands
{
    public class CalendarCommand : BaseCommand
    {
        public CalendarCommand(bool prettyPrint)
            : base(new JsonObject(), prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getCalendar";
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