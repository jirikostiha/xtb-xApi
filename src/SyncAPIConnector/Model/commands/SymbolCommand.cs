using System.Text.Json.Nodes;

namespace Xtb.XApi.Model;

public sealed class SymbolCommand : BaseCommand
{
    public const string Name = "getSymbol";

    public static readonly string[] RequiredArgs = ["symbol"];

    public SymbolCommand(JsonObject arguments, bool prettyPrint = false)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}