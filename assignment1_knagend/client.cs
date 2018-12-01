// added libraries for sockets and threading
using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace assignment1_knagned
{
    class client
    {
        Socket sckt;
        byte[] bytes;

        //Constructor which creates the TCP Socket connection to the client    
        public client(Socket socket)
        {
            sckt = socket;
        }

        //Function to send the appropriate OK response back to the client side
        public void resp(string seqNum, int sesNum)
        {
            bytes = System.Text.Encoding.UTF8.GetBytes("RTSP\\1.0 200 OK\nSeqNum: " + seqNum + "\nSesNum: " + sesNum);
            sckt.Send(bytes);
        }

        //Function to receive the information passed to the server by the client
        public byte[] messages()
        {
            byte[] receiveBuffer = new byte[4096];
            try
            {
                int rec = sckt.Receive(receiveBuffer);
                return receiveBuffer;
            }
            catch (SocketException err)
            {
                return null;
            }
        }
    }
}
