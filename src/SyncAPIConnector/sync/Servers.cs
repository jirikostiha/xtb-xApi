using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Errors;

namespace xAPI.Sync
{
    public static class Servers
    {
        /// <summary>
        /// Demo port set.
        /// </summary>
        private static PortSet DEMO_PORTS = new PortSet(5124, 5125);

        /// <summary>
        /// Real port set.
        /// </summary>
        private static PortSet REAL_PORTS = new PortSet(5112, 5113);

        /// <summary>
        /// List of all available addresses.
        /// </summary>
        private static List<ApiAddress> ADDRESSES
        {
            get
            {
                if(addresses == null)
                {
                    addresses = new List<ApiAddress>();

                    addresses.Add(new ApiAddress("xapi.xtb.com", "xAPI A"));
                    addresses.Add(new ApiAddress("xapi.xtb.com", "xAPI B"));
                }

                return addresses;
            }
        }

        /// <summary>
        /// xAPI Demo Server.
        /// </summary>
        public static Server DEMO
        {
            get
            {
                return DEMO_SERVERS[0];
            }
        }

        /// <summary>
        /// xAPI Real Server.
        /// </summary>
        public static Server REAL
        {
            get
            {
                return REAL_SERVERS[0];
            }
        }

        /// <summary>
        /// List of all demo servers.
        /// </summary>
        public static List<Server> DEMO_SERVERS
        {
            get
            {
                if (demoServers == null)
                {
                    demoServers = new List<Server>();

                    foreach (ApiAddress address in ADDRESSES)
                    {
                        demoServers.Add(new Server(address.Address, DEMO_PORTS.MainPort, DEMO_PORTS.StreamingPort, true, address.Name + " DEMO SSL"));
                    }

                    demoServers.Shuffle();
                }

                return demoServers;
            }
        }

        /// <summary>
        /// List of all real servers.
        /// </summary>
        public static List<Server> REAL_SERVERS
        {
            get
            {
                if (realServers == null)
                {
                    realServers = new List<Server>();

                    foreach (ApiAddress address in ADDRESSES)
                    {
                        realServers.Add(new Server(address.Address, REAL_PORTS.MainPort, REAL_PORTS.StreamingPort, true, address.Name + " REAL SSL"));
                    }

                    realServers.Shuffle();
                }

                return realServers;
            }
        }

        /// <summary>
        /// Gets backup server of given broken server.
        /// </summary>
        /// <param name="server">Broken server</param>
        /// <returns>Backup server</returns>
        public static Server GetBackup(Server server)
        {
            ApiAddress address = GetNextAddress(server.Address);
            return new Server(address.Address, server.MainPort, server.StreamingPort, server.Secure, address.Name);
        }

        /// <summary>
        /// Gets next API address (until the end of list).
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>Next API address</returns>
        public static ApiAddress GetNextAddress(string address)
        {
            ApiAddress apiAddress = ADDRESSES.Find(item => item.Address == address);

            if (apiAddress == null)
            {
                throw new APICommunicationException("Connection error (and no backup server available for " + address + ")");
            }
            else
            {
                // Remove the broken address
                ADDRESSES.Remove(apiAddress);

                // If there are anymore else take the first
                if (ADDRESSES.Count > 0)
                {
                    return ADDRESSES[0];
                }

                throw new APICommunicationException("Connection error (and no more backup servers available)");
            }
        }

        private static List<Server> demoServers;
        private static List<Server> realServers;
        private static List<ApiAddress> addresses;

        /// <summary>
        /// Represents a set of ports (main and streaming) for a single connection.
        /// </summary>
        public class PortSet
        {
            private int mainPort;
            private int streamingPort;

            public PortSet(int mainPort, int streamingPort)
            {
                this.mainPort = mainPort;
                this.streamingPort = streamingPort;
            }

            public int MainPort
            {
                get { return mainPort; }
            }

            public int StreamingPort
            {
                get { return streamingPort; }
            }
        }

        /// <summary>
        /// Represents a single xAPI address.
        /// </summary>
        public class ApiAddress
        {
            private string address;
            private string name;

            public ApiAddress(string address, string name)
            {
                this.address = address;
                this.name = name;
            }

            public string Address
            {
                get { return address; }
            }

            public string Name
            {
                get { return name; }
            }
        }

        /// <summary>
        /// Extends List with shuffle method.
        /// </summary>
        /// <typeparam name="T">List type</typeparam>
        /// <param name="list">List to shuffle</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
