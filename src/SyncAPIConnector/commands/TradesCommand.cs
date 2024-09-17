using System.Text.Json.Nodes;

namespace XApi.Commands;

public class TradesCommand : BaseCommand
{
    public const string Name = "getTrades";

    public static readonly string[] RequiredArgs = ["openedOnly"];

    public TradesCommand(JsonObject arguments, bool prettyPrint)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}