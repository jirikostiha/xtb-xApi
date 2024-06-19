using System.Text.Json.Nodes;
using xAPI.Errors;

namespace xAPI.Commands
{

    public abstract class BaseCommand
    {
        protected internal string commandName;
        protected internal bool? prettyPrint;
        protected internal JsonObject arguments;

        public BaseCommand(bool? prettyPrint) : this(new JsonObject(), prettyPrint)
        {
        }

        public BaseCommand(JsonObject arguments, bool? prettyPrint, string customTag = "")
        {
            this.commandName = CommandName;
            this.arguments = arguments;
            this.prettyPrint = prettyPrint;

            if (customTag == "")
                customTag = xAPI.Utils.CustomTag.Next();

            CustomTag = customTag;

            ValidateArguments();
        }

        public abstract string CommandName { get; }

        public string CustomTag { get; set; }

        public abstract string[] RequiredArguments { get; }

        public virtual bool ValidateArguments()
        {
            SelfCheck();
            foreach (string argName in RequiredArguments)
            {
                if (!this.arguments.ContainsKey(argName))
                {
                    throw new APICommandConstructionException("Arguments of [" + commandName + "] Command must contain \"" + argName + "\" field!");
                }

            }
            return true;
        }

        public virtual string ToJSONString()
        {
            JsonObject obj = new()
            {
                { "command", commandName },
                { "prettyPrint", prettyPrint },
                { "arguments", arguments },
                { "customTag", CustomTag }
            };
            return obj.ToString();
        }

        private void SelfCheck()
        {
            if (commandName == null)
            {
                throw new APICommandConstructionException("CommandName cannot be null.");
            }
            if (arguments == null)
            {
                throw new APICommandConstructionException($"Arguments cannot be null. command:'{commandName}'");
            }
        }
    }
}