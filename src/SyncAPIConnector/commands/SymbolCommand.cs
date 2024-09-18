using System.Text.Json.Nodes;

namespace Xtb.XApi.Commands;

public class SymbolCommand : BaseCommand
{
    public const string Name = "getSymbol";

    public static readonly string[] RequiredArgs = ["symbol"];

    public SymbolCommand(JsonObject arguments, bool prettyPrint)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}