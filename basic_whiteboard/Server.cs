using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace basic_whiteboard
{
    public partial class Server : Form
    {
        public Server()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        private string _port = "10000";
        public void tranData(String port)
        {
            _port = port;
            Connect();
        }
        IPEndPoint IP;
        Socket server;
        List<Socket> clientList;
        void Connect()
        {
            clientList = new List<Socket>();
            //IP  server
            int Port = Int32.Parse(_port);
            IP = new IPEndPoint(IPAddress.Any, Port);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            server.Bind(IP);

            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(100);
                        Socket client = server.Accept();
                        if (clientList.Count < 4)
                        {
                            clientList.Add(client);
                        }
                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(client);
                    }
                }
                catch
                {
                    //IP server
                    IP = new IPEndPoint(IPAddress.Any, 9900);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                }

            });

            Listen.IsBackground = true;
            Listen.Start();
        }
        void Receive(object obj)
        {
            
        }

        private void Server_Load(object sender, EventArgs e)
        {
            MongoGRUD db = new MongoGRUD("WhiteBoard"); //call Server          
        }

        public class MongoGRUD
        {
            private IMongoDatabase db;
            public MongoGRUD(string database)
            {
                var client = new MongoClient("mongodb://127.0.0.1:27017");
                db = client.GetDatabase(database);
            }
            public void InsertRecords<T>(string table, T record)
            {
                var collection = db.GetCollection<T>(table);
                collection.InsertOne(record);
            }
        }
    }
}
