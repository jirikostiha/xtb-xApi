using System.Text.Json.Nodes;

namespace xAPI.Commands;

public class LogoutCommand : BaseCommand
{
    public const string Name = "logout";

    public LogoutCommand()
        : base([], false)
    {
    }

    public override string ToJSONString()
    {
        JsonObject obj = new()
        {
            { "command", CommandName }
        };

        return obj.ToString();
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => [];
}