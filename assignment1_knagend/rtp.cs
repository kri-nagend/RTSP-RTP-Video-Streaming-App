// added libraries for sockets, timers and IO
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace assignment1_knagned
{
    class rtp
    {
        // initializing and declaring variables
        int frmNum = 0;
        public Socket sckCon;
        public string vidFile;
        byte[] buff = new byte[4096];
        FileStream vid;
        IPEndPoint cltEP;
        Timer count;
        rtppacket RTPpckt;
        

        // class constructor
        public rtp()
        {
            // sets frame to 0; and starts timer
            frmNum = 0;
            count = new Timer();
            count.Elapsed += new ElapsedEventHandler(sendRTPpckt);
            count.Interval = 100;
        }

        // accessor function to get frame number
        public int frm()
        {
            return frmNum;
        }

        // start connection function
        public void strConn(string portNo)
        {
            int port = int.Parse(portNo);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            cltEP = new IPEndPoint(ipAddress, port);
            sckCon = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Dgram,
            ProtocolType.Udp
            );
            RTPpckt = new rtppacket();
        }

        // video setup function
        public void streamVid(string filename)
        {
            vidFile = filename;
            // the first parameter of this function call must be changed to your video path
            vid = new FileStream("C:\\Users\\Kri\\Desktop\\School\\SE 3314 Computer Network Applications\\Assignment1\\assignment1_knagned\\" + filename, FileMode.Open, FileAccess.Read);
        }
                
        // timer function; called on play
        public void resumeTime()
        {
            count.Enabled = true;
        }

        // stop function; called on pause
        public void stop()
        {
            count.Stop();
        }

        // end connection function; called on teardown
        public void endCon()
        {
            sckCon.Close();
        }

        public string head()
        {
            return Encoding.Default.GetString(RTPpckt.header());
        }

        // sends the video frames to the client
        public void sendRTPpckt(Object source, ElapsedEventArgs e)
        {
            // increment frame counter, and initalize the variables
            frmNum++;
            int sz;
            int init = 5;
            byte[] ary = new byte[init];
            //
            vid.Read(ary, 0, 5);
            sz = Int32.Parse(System.Text.Encoding.UTF8.GetString(ary));
            //creating a byte array to store those frame
            byte[] frame = new byte[sz];
            vid.Read(frame, 0, sz);
            // checks if there is a frame to send, or it closes the connection
            if (frame != null)
            {
                sckCon.SendTo(RTPpckt.encap(frame), cltEP);
            }
            else
            {
                sckCon.Close();
                count.Enabled = false;
            }
        }
    }
}
