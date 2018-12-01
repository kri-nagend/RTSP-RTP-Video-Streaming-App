using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace assignment2_knagend
{
    class rtsp
    {
        // initializing the variables
        IPAddress IP;
        Socket sckt;
        IPEndPoint endPoint;
        int portNum;
        
        // function to set the IP address from the Form
        public void setAdr(string adr)
        {
            IP = IPAddress.Parse(adr);
        }
        // function to set the port number from the Form
        public void setportNum(int port)
        {
            portNum = port;
        }

        // function to make server requests
        public string serReq(String request)
        {
            // converts request to bytes and sends it to server
            sckt.Send(System.Text.Encoding.UTF8.GetBytes(request));

            // array to store responses
            byte[] resp = new byte[4096];
            try
            {
                // recevies server response and sends to controller
                int recv = sckt.Receive(resp);
                return System.Text.Encoding.UTF8.GetString(resp);
            }
            catch (Exception e)
            {
                // returns nothing if error
                return "";
            }
        }

        // function to make the socket
        public void create()
        {
            endPoint = new IPEndPoint(IP, portNum);
            sckt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        // function to make a connection to the socket
        public bool cnct()
        {
            // connects the socket to the end point
            try
            {
                sckt.Connect(endPoint);
            }
            catch (Exception e)
            {
                return false;
            }
            // returns true if the socket is connected else false
            return sckt.Connected;
        }

    }
}
