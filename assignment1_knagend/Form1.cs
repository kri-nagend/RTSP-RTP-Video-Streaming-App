// added libraries for timers, sockets
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace assignment1_knagned
{
    public partial class Form1 : Form
    {
        // initializing the delegates
        delegate void IPAddress(string ip);
        delegate void serverText(string server);
        delegate void clientText(string client);
        delegate void checkBox(string check);
        delegate void frameNum(string frame);
        
        // initializing the form so that only the Port box can be edited
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
        }
        
        // initialzing form
        public Form1()
        {
            InitializeComponent();
            controller control;
            control = new controller();
            // adding functionality to the header check, and button
            this.button1.Click += new System.EventHandler(control.btn1);
            this.checkBox1.CheckedChanged += new EventHandler(control.clc);
        }

        ////
        //// Functions responsible for the IP Address
        ////
        // Function adds the IP address to textBox1
        public void editAddr(String text)
        {
            this.textBox1.Text = text;
        }
        // Function calls the delegate and create the text to add to the field
        public void AddressBox(String message)
        {
            string text = message;
            IPAddress obj = new IPAddress(editAddr);
            this.Invoke(obj, new object[] { text });
        }

        ////
        //// Functions responsible for the frame number
        ////
        // Function updates the box with the current frame number
        public void upFrm(String num)
        {
            this.textBox2.Text = num;
        }
        // Function calls the delegates and retrieves the frame number
        public void SetFrameNumberInfoBox(String nom)
        {
            string text = nom;
            frameNum d = new frameNum(upFrm);
            this.Invoke(d, new object[] { text });
        }

        ////
        //// Functions responsible of the client box
        ////
        // Function adds the new client info to the box
        public void upClient(String text)
        {
            // use += to not erase the previous info
            this.textBox5.Text += text;
        }
        // Function calls the delegate
        public void SetClientInfoBox(String info)
        {
            string text1 = info;
            clientText obj = new clientText(upClient);
            this.Invoke(obj, new object[] { text1 });
        }

        ////
        //// Functions responsible for the server box
        ////
        // Function updates the server text box
        public void upServer(String text)
        {
            // use += to not erase the previous info
            this.textBox4.Text += text;
        }
        // Function calls the delegate
        public void SetServerInfoBox(String msg)
        {
            string text = msg;
            serverText d = new serverText(upServer);
            this.Invoke(d, new object[] { text });
        }
        
        // Is a function to get the port number
        public int portNum()
        {
            int prt;
            prt = Int32.Parse(textBox3.Text);
            return prt;
        }
        // To not allow listen to be pressed twice 
        public void lockBTN()
        {
            this.button1.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
