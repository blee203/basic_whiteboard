using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
namespace basic_whiteboard
{
    public partial class WorkSpace : Form
    {
        public string code;
        public string name;
        public string _port = "9090";
        public WorkSpace()
        {
            InitializeComponent();
        }
        public void funData(string str, string text, string port)
        {
            code = str;
            name = text;
            _port = port;
            Connect();
        }

        IPEndPoint IP;
        Socket Client;

        void Connect()
        {
            //IP: server address
            int Port = Int32.Parse(_port);
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                Client.Connect(IP);
            }
            catch
            {
                MessageBox.Show("Can't connect", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
                return;
            }

            Thread listener = new Thread(Receive);
            listener.IsBackground = true;
            listener.Start();
        }

        void Receive()
        {

        }

        private void WorkSpace_Load(object sender, EventArgs e)
        {
            textBox1.Text = code;
        }
    }
}
