using System.Text.Json.Nodes;

namespace Xtb.XApi.Model;

public sealed class LoginCommand : BaseCommand
{
    public const string Name = "login";

    public static readonly string[] RequiredArgs = ["userId", "password"];

    public LoginCommand(JsonObject arguments, bool prettyPrint = false)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}