using System.Text.Json.Nodes;

namespace Xtb.XApi.Model;

public sealed class CommissionDefCommand : BaseCommand
{
    public const string Name = "getCommissionDef";

    public static readonly string[] RequiredArgs = ["symbol", "volume"];

    public CommissionDefCommand(JsonObject arguments, bool prettyPrint = false)
        : base(arguments, prettyPrint)
    {
    }

    public override string ToJsonString()
    {
        JsonObject obj = new()
        {
            { "command", CommandName },
            { "arguments", Arguments },
            { "extended", true }
        };

        return obj.ToString();
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}