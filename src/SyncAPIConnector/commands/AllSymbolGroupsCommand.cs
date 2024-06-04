using System;
using System.Text.Json.Nodes;

namespace xAPI.Commands
{
    public class AllSymbolGroupsCommand : BaseCommand
    {
        [Obsolete("Not available in API any more")]
        public AllSymbolGroupsCommand(bool? prettyPrint)
            : base(new JsonObject(), prettyPrint)
        {
        }

        public override string CommandName
        {
            get { return "getAllSymbolGroups"; }
        }

        public override string[] RequiredArguments
        {
            get { return []; }
        }
    }

}