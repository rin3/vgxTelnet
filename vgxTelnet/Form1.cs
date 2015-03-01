using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;

namespace vgxTelnet
{
    public partial class Form1 : Form
    {
        // server variables
        static public string host;
        static public int port;
        
        // the server selected: 1..5
        static public int svr;
        
        AboutBox1 abt;
        Form2 setform;

        // Socket
        System.Net.Sockets.TcpClient sck;
        // Network stream
        System.Net.Sockets.NetworkStream ns;

        public Form1()
        {
            InitializeComponent();

            abt = new AboutBox1();
            setform = new Form2();

            // get the selected server
            Form2.update_SelectedServer();

            // get font and colors
            rcvTextBox.Font = Properties.Settings.Default.fFont;
            rcvTextBox.ForeColor = Properties.Settings.Default.fColor;
            rcvTextBox.BackColor = Properties.Settings.Default.bColor;

            Form1.form1Instance = this;
        }

        // for external components access
        private static Form1 _form1Instance;

        public static Form1 form1Instance
        {
            get { return _form1Instance; }
            set { _form1Instance = value; }
        }

        // rcvTextBox.Font public stub
        public Font rcvFont
        {
            get { return rcvTextBox.Font; }
            set { rcvTextBox.Font = value; }
        }

        // rcvTextBox.ForeColor stub
        public Color fontColor
        {
            get { return rcvTextBox.ForeColor; }
            set { rcvTextBox.ForeColor = value; }
        }
        
        // rcvTextBox.BackColor stub
        public Color bckColor
        {
            get { return rcvTextBox.BackColor; }
            set { rcvTextBox.BackColor = value; }
        }
        
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void statusRed()
        {
            // stop timer
            timer1.Stop();

            // show red alert in statusbar
            statusLabel0.Image = Properties.Resources.RedMinus;
        }
        
        private void sendLine(NetworkStream ns0, string str0)
        {
            try
            {
                if (ns0.CanWrite)
                {
                    byte[] sendb = System.Text.Encoding.ASCII.GetBytes(str0);
                    ns0.Write(sendb, 0, sendb.Length);
                }
            }
            catch (IOException ex)
            {
                // status message, as error
                statusRed();
                statusLabel1.Text = "IO ERROR";
                rcvTextBox.AppendText(ex.Message + "\n");
            }
        }

        private void readChunk(NetworkStream ns0)
        {
            // Detect if client disconnected
            if (sck.Client.Poll(0, SelectMode.SelectRead))
            {
                byte[] buff = new byte[1];
                try
                {
                    if (sck.Client.Receive(buff, SocketFlags.Peek) == 0)
                    {
                        // Client disconnected
                        discon();
                    }
                }
                catch (SocketException ex)
                {
                    // status message, as error
                    statusRed();
                    statusLabel1.Text = "Socket ERROR";
                    rcvTextBox.AppendText(ex.Message + "\n");
                }
            }

            try
            {
                if (ns0.DataAvailable)
                {
                    byte[] recvb = new byte[sck.ReceiveBufferSize];
                    ns0.Read(recvb, 0, recvb.Length);

                    rcvTextBox.AppendText(System.Text.Encoding.ASCII.GetString(recvb));
                }
            }
            catch(ObjectDisposedException)
            {
                discon();
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            sendText();
        }

        private void sendTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                sendText();
            }
        }

        private void sendText()
        {
            if (ns != null)
            {
                sendLine(ns, sendTextBox.Text + "\r\n");
                sendTextBox.Clear();
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (host == null || host == "" || port == 0)
            {
                // show warning
                statusRed();
                statusLabel1.Text = "Server settings ERROR";
                statusLabel2.Text = "";
                return;
            }
            
            // Connect to the server
            try
            {
                // status message
                statusLabel0.Image = null;
                statusLabel1.Text = "Trying ...";
                statusLabel2.Text = string.Format("{0}:{1}", host, port);
                Update(); // Refresh() not needed

                // open a socket
                sck = new System.Net.Sockets.TcpClient(host, port);
                // Create stream
                ns = sck.GetStream();
                //ns.ReadTimeout = 1000;

                // status message, as connected
                statusLabel0.Image = Properties.Resources.GreenCheck;
                statusLabel1.Text = "Connected";
                // Timer start
                timer1.Start();
            }
            catch (SocketException ex)
            {
                // status message, as error
                statusRed();
                statusLabel1.Text = "Socket ERROR";
                rcvTextBox.AppendText(ex.Message+"\n");
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            discon();   
        }

        private void discon()
        {
            // Timer close down
            timer1.Stop();
            // Disconnect from the server
            if (sck != null)
            {
                ns.Close();
                sck.Close();
            }
            statusLabel0.Image = null;
            statusLabel1.Text = "Disconnected";
            statusLabel2.Text = "";
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abt.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            readChunk(ns);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setform.ShowDialog();
        }

        private void clearScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rcvTextBox.Clear();
        }
    }
}
