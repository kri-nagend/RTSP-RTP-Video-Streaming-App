using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace assignment2_knagend
{
    class controller
    {
        // declaring the variables
        Form1 GUI;
        System.Timers.Timer tmr;
        rtp rtp;
        rtsp rtsp;
        int seqNum;
        string sesTxt = "";
        bool IsPaused, IsTeared;

        // class  constructor/ setting up the timer
        public controller()
        {
            tmr = new System.Timers.Timer();
        }

        // function reacts to connect button
        public void conPres(object sender)
        {
            // initalizes From
            GUI = (Form1)((Button)sender).FindForm();

            // creates rtsp, and assigns port and IP to the ones from Form1, then creates the socket
            rtsp = new rtsp();
            rtsp.setportNum(GUI.portNum());
            rtsp.setAdr(GUI.IPAdr());
            rtsp.create();
            
            // tests the connection, and calls the function
            if (rtsp.cnct())
            {
                GUI.sCon();
                System.Console.Write("hi");
            }
        }

        // functions reacts to the setup button
        public void setupPres()
        {
            // sets sequence number to 1, and sets the bools 
            seqNum = 1;
            IsPaused = false;
            IsTeared = false;

            // sends the setup resquest to the sever, and stores the response
            string resp = rtsp.serReq("SETUP rtsp://" + GUI.IPAdr() + ":" + GUI.portNum() + "/" + GUI.getVid() + " RTSP/1.0\r\nCSeq: " + seqNum + "\r\nTransport: RTP/UDP; client_port= 2097\r\n");

            // checks if the server responded 
            if (resp != "")
            {
                // parses the response to find the server session number
                char[] delimiterChars = { ' ', ',', ';', ':', '/', '\n', '\r' };
                string[] words = resp.Split(delimiterChars);
                sesTxt = words[11];
                GUI.ServerInfo(resp + "\r\n");

                // creates an rtp object with the port number and IP address from Form1
                rtp = new rtp();
                rtp.setPort(GUI.portNum());
                rtp.setIP(GUI.IPAdr());
                rtp.create();
            }
        }

        // function reacts to the play button
        public void playPres()
        {
            // increments sequence number to 1, and sets the bools 
            seqNum++;
            IsPaused = false;
            IsTeared = false;

            // sends the setup resquest to the sever, and stores the response
            string response = rtsp.serReq("PLAY rtsp://" + GUI.IPAdr() + ":" + GUI.portNum() + "/" + GUI.getVid() + " RTSP/1.0\r\nCSeq: " + seqNum + "\r\nSession: " + sesTxt + "\r\n");

            // checks if the server responded 
            if (response != "")
            {
                // starts the timer, and updates the server box using the delegate
                tmr.Elapsed += new ElapsedEventHandler(NewFrame);
                tmr.Interval = 60;
                tmr.Enabled = true;
                GUI.ServerInfo(response + "\r\n");
            }
        }
        
        // function reacts to the pause button
        public void pausePres()
        {
            // increments sequence number to 1, and sets the bools 
            seqNum++;
            IsPaused = true;
            IsTeared = false;

            // sends the setup resquest to the sever, and stores the response
            string response = rtsp.serReq("PAUSE rtsp://" + GUI.IPAdr() + ":" + GUI.portNum() + "/" + GUI.getVid() + " RTSP/1.0\r\nCSeq: " + seqNum + "\r\nSession: " + sesTxt + "\r\n");

            // checks if the server responded 
            if (response != "")
            {
                // pauses the timer, and updates the server box using the delegate
                tmr.Stop();
                GUI.ServerInfo(response + "\r\n");
            }
        }

        // functions reacts to the teardown button
        public void teardownPres()
        {
            // increments sequence number to 1, and sets the bools 
            seqNum++;
            IsPaused = false;
            IsTeared = true;

            // sends the setup resquest to the sever, and stores the response
            string response = rtsp.serReq("TEARDOWN rtsp://" + GUI.IPAdr() + ":" + GUI.portNum() + "/" + GUI.getVid() + " RTSP/1.0\r\nCSeq: " + seqNum + "\r\nSession: " + sesTxt + "\r\n");

            // checks if the server responded 
            if (response != "")
            {
                // pauses the timer, and updates the server box using the delegate, and closes the connection
                tmr.Stop();
                GUI.ServerInfo(response + "\r\n");
                rtp.close();
            }
        }

        // function updates the screen as the timer increments 
        public void NewFrame(object source, ElapsedEventArgs e)
        {
            // stores the image information in an array
            byte[] img = rtp.upScrn();

            // checks if the video should be stopped 
            if (img == null && IsPaused == false && IsTeared == false)
            {
                // if it should the timer is stopped, the connection is closed, and teardown is called
                tmr.Enabled = false;
                rtp.close();
                GUI.SetTeardown();
                return;
            }
            // if the video can continue, the new image is sent to Form1
            else if (img != null)
            {
                using (var ms = new MemoryStream(img))
                {
                    GUI.NewImage(Image.FromStream(ms));
                }
            }
        }
    }
}
