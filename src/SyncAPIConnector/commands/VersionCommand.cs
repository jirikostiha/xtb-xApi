namespace xAPI.Commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class VersionCommand : BaseCommand
    {
        public VersionCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getVersion";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { };
            }
        }
    }

}