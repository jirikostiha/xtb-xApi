using System.Text.Json.Nodes;

namespace xAPI.Commands;

public class StepRulesCommand : BaseCommand
{
    public StepRulesCommand()
        : base(new JsonObject(), false)
    {
    }

    public override string CommandName => "getStepRules";

    public override string[] RequiredArguments => [];
}