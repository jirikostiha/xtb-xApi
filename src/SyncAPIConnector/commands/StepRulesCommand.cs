using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class StepRulesCommand : BaseCommand
    {
        public StepRulesCommand()
            : base(new JsonObject(), false)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getStepRules";
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