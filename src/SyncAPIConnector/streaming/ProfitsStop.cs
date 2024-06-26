﻿using System.Text.Json.Nodes;

namespace xAPI.Streaming
{

    sealed class ProfitsStop
    {
        public ProfitsStop()
        {
        }

        public override string ToString()
        {
            JsonObject result = new()
            {
                { "command", "stopProfits" }
            };
            return result.ToJsonString();
        }
    }
}
