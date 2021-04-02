using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerP2P
{
    class Client
    {
        public Socket Connector = null; // Each client will have its own binded socket for connection to server.
 
        public String IP = "";
        public String name = "";
        public int Port = -1; // Initially the client's port is unknown.
    }
}
