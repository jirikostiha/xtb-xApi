using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

public sealed class TradesCommand : BaseCommand
{
    public const string Name = "getTrades";

    public static readonly string[] RequiredArgs = ["openedOnly"];

    public TradesCommand(JsonObject arguments, bool prettyPrint = false)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}