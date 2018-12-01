// added libraries for sockets 
using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace assignment1_knagned
{
    class rtsp
    {
        // initializing variables
        int portNum;
        IPAddress IP;
        Socket Server;

        // accessor function to get the address
        public IPAddress rtnAddr()
        {
            return IP;
        }

        // class constructor
        public rtsp(int portNo)
        {
            portNum = portNo;
            IP = IPAddress.Parse("127.0.0.1");
            // sets up the server with a tcp connection
            IPEndPoint endConc = new IPEndPoint(IP, portNum);
            Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Server.Bind(endConc);
            Server.Listen(int.MaxValue);

        }
             
        // function for when the connection has been accepted
        public Socket ContcAccp()
        {
            Socket Client;
            try
            {
                // accepting the connection
                Client = Server.Accept();
                Client.RemoteEndPoint.ToString();
                return Client;
            }
            // in the case of an error
            catch (SocketException err)
            {
                return null;
            }
            
        }
    }
}