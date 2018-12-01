using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assignment2_knagend
{
    public partial class Form1 : Form
    {
        // declaring the variables and delegates
        controller ctrl;
        delegate void SetButtonConfig();
        delegate void SetScreen(Image i);
        delegate void setServerInfo(string data);

        // class constructor
        public Form1()
        {
            InitializeComponent();
            // initializing the controller
            ctrl = new controller();
        }

        //
        //// Button functions 
        //

        // function controls the connect button
        private void button1_Click(object sender, EventArgs e)
        {
            ctrl.conPres(sender);
        }

        // function controls the exit button
        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        // function controls the setup button
        private void button3_Click(object sender, EventArgs e)
        {
            // calls setup function/ enables play button/ disables setup button
            ctrl.setupPres();
            button4.Enabled = true;
            button3.Enabled = false;
        }
        
        // function controls the play button
        private void button4_Click(object sender, EventArgs e)
        {
            // calls play function/ enables pause/teardown button/ disables play button
            ctrl.playPres();
            button5.Enabled = true;
            button6.Enabled = true;
            button4.Enabled = false;

        }

        // function controls the pause button
        private void button5_Click(object sender, EventArgs e)
        {
            // calls pause function/ enables play button/ disables pause button
            ctrl.pausePres();
            button4.Enabled = true;
            button5.Enabled = false;
        }

        // function controls the teardown button
        private void button6_Click(object sender, EventArgs e)
        {
            // calls teardown function/ enables setup button/ disables play/pause/teardown button
            ctrl.teardownPres();
            button3.Enabled = true;
            button6.Enabled = false;
            button5.Enabled = false;
            button4.Enabled = false;

        }

        //
        //// Input box functions
        //
        // function sends video name to the controller
        public string getVid()
        {
            return sele.Text;
        }
        // function sends the port num to the controller
        public int portNum()
        {
            return int.Parse(textBox4.Text);
        }
        // function sends the IP address to the controller
        public string IPAdr()
        {
            return textBox5.Text;
        }


        // function called when the connection is successful
        public void sCon()
        {
            // enables the setup button/ disables the connect button
            button3.Enabled = true;
            button1.Enabled = false;
        }

        //
        //// Delegate functions 
        //

        // function to tearndown video
        public void SetTeardown()
        {
            this.Invoke(new SetButtonConfig(teardown));
        }
        public void teardown()
        {
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
        }

        // function to update the screen
        public void NewImage(Image img)
        {
            this.Invoke(new SetScreen(upScreen), new object[] { img });
        }
        public void upScreen(Image imag)
        {
            this.screen.Image = imag;
        }

        // function to update the server box
        public void ServerInfo(String ServerM)
        {
            this.Invoke(new setServerInfo(upSeverbox), new object[] { ServerM });
        }
        public void upSeverbox(String res)
        {
            this.textBox7.Text += res;
        }

        // function to update the header box
        public void HeaderInfo(String Head)
        {
            this.Invoke(new setServerInfo(upHeaderbox), new object[] { Head });
        }
        public void upHeaderbox(String hd)
        {
            this.textBox6.Text += hd;
        }
    }
}

