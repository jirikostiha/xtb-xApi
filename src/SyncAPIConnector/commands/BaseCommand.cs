using System.Text.Json.Nodes;
using xAPI.Errors;

namespace xAPI.Commands
{
    public abstract class BaseCommand : ICommand
    {
        protected internal bool? PrettyPrint { get; set; }

        protected BaseCommand(bool? prettyPrint)
            : this([], prettyPrint)
        {
        }

        protected BaseCommand(JsonObject arguments, bool? prettyPrint, string customTag = "")
        {
            Arguments = arguments;
            PrettyPrint = prettyPrint;

            if (customTag == "")
                customTag = Utils.CustomTag.Next();

            CustomTag = customTag;

            ValidateArguments();
        }

        public abstract string CommandName { get; }

        public JsonObject Arguments { get; protected set; }

        public string CustomTag { get; set; }

        public abstract string[] RequiredArguments { get; }

        public virtual bool ValidateArguments()
        {
            SelfCheck();
            foreach (string argName in RequiredArguments)
            {
                if (!Arguments.ContainsKey(argName))
                {
                    throw new APICommandConstructionException("Arguments of [" + CommandName + "] Command must contain \"" + argName + "\" field!");
                }
            }

            return true;
        }

        public virtual string ToJSONString()
        {
            JsonObject obj = new()
            {
                { "command", CommandName },
                { "prettyPrint", PrettyPrint },
                { "arguments", Arguments },
                { "customTag", CustomTag }
            };

            return obj.ToString();
        }

        private void SelfCheck()
        {
            if (CommandName == null)
            {
                throw new APICommandConstructionException("CommandName cannot be null.");
            }

            if (Arguments == null)
            {
                throw new APICommandConstructionException($"Arguments cannot be null. command:'{CommandName}'");
            }
        }
    }
}