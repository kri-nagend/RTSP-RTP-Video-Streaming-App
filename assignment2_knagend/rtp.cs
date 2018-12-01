using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace assignment2_knagend
{
    class rtp
    {
        // initialize the variables
        int portNum;
        Socket sock;
        IPEndPoint EP;
        rtppacket Rpkt;
        IPAddress IP;
       
        // class constructor
        public rtp()
        {
            Rpkt = new rtppacket();
        }

        // function to assign the address
        public void setIP(string adr)
        {
            IP = IPAddress.Parse(adr);
        }

        // function to set the port number
        public void setPort(int port)
        {
            portNum = port;
        }
        
        // function to create a new socket
        public void create()
        {
            EP = new IPEndPoint(IP, 2097);
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.Bind(EP);
        }

        // function to get packets header info
        public string getHead()
        {
            return Rpkt.Header();
        }

        // function to close the socket 
        public void close()
        {
            sock.Close();
        }

        // function to update the screen and parse the arrays 
        public byte[] upScrn()
        {
            try
            {
                // array to store image info
                byte[] image = new byte[100000];

                // sets the timeout value
                sock.ReceiveTimeout = 5000;
                try
                {
                    // receiver server info
                    EndPoint endP = EP;
                    sock.ReceiveFrom(image, ref endP);

                    // splits up the header info
                    byte[] noHead = new byte[100000];
                    noHead = Rpkt.seperate(image);
                    return noHead;
                }

                // the error scenarios 
                catch (SocketException e)
                {
                    
                }
            }
            catch
            {
                return null;
            }
            return null;
        }       
    }
}
