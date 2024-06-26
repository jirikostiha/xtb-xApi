using System.Text.Json.Nodes;
using xAPI.Commands;

namespace xAPI.Streaming
{
    public sealed class BalanceRecordsStop : ICommand
    {
        public const string Name = "stopBalance";

        public string CommandName => Name;

        public string[] RequiredArguments => [];

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", CommandName }
            };

            return result.ToJsonString();
        }
    }
}