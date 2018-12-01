// added libraries for sockets
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace assignment1_knagned
{
    class controller
    {
        // initializes the variables
        private static Form1 form;
        public
        int sessionNo = 0;
        int frm;
        int sesC;
        bool prntHead = false; 
        Thread clientThr;
        string cmd;
        string vid;
        string port;
        string seq;
        string temp;
               

        // Listen button funcationality
        public void btn1(object sender, EventArgs e)
        {
            form = (Form1)((Button)sender).FindForm();
            Thread lisBtn = new Thread(openConnection);
            lisBtn.IsBackground = true;
            lisBtn.Start();
            form.lockBTN();
        }
        // Printer Header check box funcationality
        public void clc(object sender, EventArgs e)
        {
            if (prntHead == false)
            {
                prntHead = true;
            }
            else
            {
                prntHead = false; 
            } 
        }

        // Function to open the connection; called by the listen button
        private void openConnection()
        {
            // initalizes connection with the given port number and finds the IP
            rtsp cnt = new rtsp(form.portNum());
            form.AddressBox(cnt.rtnAddr().ToString());
            
            // Forever loop that accepts incoming connections
            while (true)
            {
                form.SetServerInfoBox("Waiting on a new connection.... " + "\r\n");
                Socket newCnt = cnt.ContcAccp();
                form.SetServerInfoBox("Client has connected on: " + newCnt.RemoteEndPoint.ToString() + "\r\n");
                clientThr = new Thread(new ParameterizedThreadStart(comms));
                clientThr.IsBackground = true;
                clientThr.Start(newCnt);
            }
        }
       
        // Creates new client for each new connection
        private void comms(Socket skt)
        {
            // initlizes new rtp and client object
            rtp rtpCon = new rtp();
            frm = rtpCon.frm();
            client cltCon = new client(skt);

            // forever loop to check for new intructions
            while (true)
            {
                try
                {
                    // parsing the client messages to find instructions and other info
                    string instr = Encoding.UTF8.GetString(cltCon.messages());
                    // find the unnesscary characters
                    char[] breakPoints = { '\r', '\n', '/', ',', ';', ':', ' '};
                    string[] info = instr.Split(breakPoints);
                    if (info.Length > 20)
                    {
                        port = info[20];
                        cmd = info[0];
                        vid = info[6];
                        seq = info[12];
                    }
                    // if the message is large it takes a different format
                    else if (info.Length > 12)
                    {
                        cmd = info[0];
                        vid = info[6];
                        seq = info[12];
                        temp = info[17];
                        sesC = int.Parse(temp);
                    }

                    form.SetClientInfoBox(instr);

                    // executing the instructions found from the client messages
                    if (cmd == "SETUP")
                    {
                        // increments session # and displays the status in the server info box
                        sessionNo++;
                        form.SetClientInfoBox(vid);
                        form.SetServerInfoBox("Client " + sessionNo+ " has step up video\r\n");
                        rtpCon.strConn(port);
                        rtpCon.streamVid(vid);
                        cltCon.resp(seq, sessionNo);
                    }
                    else if (cmd == "TEARDOWN")
                    {
                        // ends the connection and updats the server box
                        form.SetServerInfoBox("Client " + sessionNo + " has closed the connection\r\n");
                        cltCon.resp(seq, sesC);
                        rtpCon.stop();
                        rtpCon.vidFile = "";
                        rtpCon.endCon();
                    }
                    else if (cmd == "PLAY")
                    {
                        // starts the timer and updates the server info box, and frames
                        form.SetClientInfoBox(seq);
                        form.SetServerInfoBox("Client " + sessionNo + " is playing the video\r\n");
                        if(prntHead)
                        {
                            form.SetServerInfoBox("Print Header\r\n");
                        }
                        form.SetFrameNumberInfoBox(rtpCon.frm().ToString());
                        cltCon.resp(seq, sesC);
                        rtpCon.resumeTime();                     
                    }
                    else if (cmd == "PAUSE")
                    {
                        // pauses the connection and video, and updates the server info box
                        form.SetServerInfoBox("Client " + sessionNo + " has paused the video\r\n");
                        form.SetFrameNumberInfoBox(rtpCon.frm().ToString());
                        cltCon.resp(seq, sesC);
                        rtpCon.stop();
                    }
                }
                catch (Exception err)
                {
                    // N/A
                }
            }
        }
        // function call
        private void comms(Object skt)
        {
            comms((Socket)skt);
        }

    }
}
