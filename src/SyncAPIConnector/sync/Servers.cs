using System;
using System.Collections.Generic;

namespace xAPI.Sync;

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

    private static List<Server>? _demoServers;
    private static List<Server>? _realServers;
    private static List<ApiAddress>? _addresses;

    /// <summary>
    /// List of all available addresses.
    /// </summary>
    private static List<ApiAddress> ADDRESSES
    {
        get
        {
            if (_addresses == null)
            {
                _addresses = [
                    new ApiAddress("xapi.xtb.com", "xAPI A"),
                    new ApiAddress("xapi.xtb.com", "xAPI B")];
            }

            return _addresses;
        }
    }

    /// <summary>
    /// xAPI Demo Server.
    /// </summary>
    public static Server DEMO => DEMO_SERVERS[0];

    /// <summary>
    /// xAPI Real Server.
    /// </summary>
    public static Server REAL => REAL_SERVERS[0];

    /// <summary>
    /// List of all demo servers.
    /// </summary>
    public static List<Server> DEMO_SERVERS
    {
        get
        {
            if (_demoServers == null)
            {
                _demoServers = new List<Server>();

                foreach (ApiAddress address in ADDRESSES)
                {
                    _demoServers.Add(new Server(address.Address, DEMO_PORTS.MainPort, DEMO_PORTS.StreamingPort, true, address.Name + " DEMO SSL"));
                }

                _demoServers.Shuffle();
            }

            return _demoServers;
        }
    }

    /// <summary>
    /// List of all real servers.
    /// </summary>
    public static List<Server> REAL_SERVERS
    {
        get
        {
            if (_realServers == null)
            {
                _realServers = [];

                foreach (ApiAddress address in ADDRESSES)
                {
                    _realServers.Add(new Server(address.Address, REAL_PORTS.MainPort, REAL_PORTS.StreamingPort, true, address.Name + " REAL SSL"));
                }

                _realServers.Shuffle();
            }

            return _realServers;
        }
    }

    /// <summary>
    /// Gets backup server of given broken server.
    /// </summary>
    /// <param name="server">Broken server</param>
    /// <returns>Backup server</returns>
    public static Server? GetBackup(Server server)
    {
        ApiAddress address = GetNextAddress(server.Address);
        if (address == null)
        {
            return null;
        }
        return new Server(address.Address, server.MainPort, server.StreamingPort, server.IsSecure, address.Name);
    }

    /// <summary>
    /// Gets next API address (until the end of list).
    /// </summary>
    /// <param name="address">Address</param>
    /// <returns>Next API address</returns>
    public static ApiAddress? GetNextAddress(string address)
    {
        ApiAddress apiAddress = ADDRESSES.Find(item => item.Address == address);

        if (apiAddress == null)
        {
            return null;
            //throw new APICommunicationException("Connection error (and no backup server available for " + address + ")");
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

            return null;
            //throw new APICommunicationException("Connection error (and no more backup servers available)");
        }
    }

    /// <summary>
    /// Represents a set of ports (main and streaming) for a single connection.
    /// </summary>
    public class PortSet
    {
        public PortSet(int mainPort, int streamingPort)
        {
            MainPort = mainPort;
            StreamingPort = streamingPort;
        }

        public int MainPort { get; }

        public int StreamingPort { get; }
    }

    /// <summary>
    /// Represents a single xAPI address.
    /// </summary>
    public class ApiAddress
    {
        public ApiAddress(string address, string name)
        {
            Address = address;
            Name = name;
        }

        public string Address { get; }

        public string Name { get; }
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