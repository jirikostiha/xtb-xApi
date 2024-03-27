using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Sync
{
    public class Server
    {
        private string address;
        private int mainPort;
        private int streamingPort;
        private string description;
        private bool secure;

        public Server(string address, int mainPort, int streamingPort, bool secure, string description)
        {
            this.address = address;
            this.mainPort = mainPort;
            this.streamingPort = streamingPort;
            this.secure = secure;
            this.description = description;
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int MainPort
        {
            get { return mainPort; }
            set { mainPort = value; }
        }

        public int StreamingPort
        {
            get { return streamingPort; }
            set { streamingPort = value; }
        }

        public bool Secure
        {
            get { return secure; }
            set { secure = value; }
        }

        public override string ToString()
        {
            return Description + " (" + Address + ":" + MainPort + "/" + StreamingPort + ")";
        }
    }
}
