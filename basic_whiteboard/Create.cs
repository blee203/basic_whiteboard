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
namespace basic_whiteboard
{
    public partial class Create : Form
    {
        public Create()
        {
            InitializeComponent();
        }
        struct MyRoom
        {
            public string Code;
            public string Port;
            public MyRoom(String Code, String Port)
            {
                this.Code = Code;
                this.Port = Port;
            }
        }
        // list of room
        static List<MyRoom> List_room = new List<MyRoom>()
        {
            new MyRoom("room1","9901"),
            new MyRoom("room2","9902"),
            new MyRoom("room3","9903"),
            new MyRoom("room4","9904"),
            new MyRoom("room5","9905"),
            new MyRoom("room6","9906"),
            new MyRoom("room7","9907"),
            new MyRoom("room8","9908"),
            new MyRoom("room9","9909"),
            new MyRoom("room0","9900"),

        };

        private WorkSpace workSpace = new WorkSpace();
        private Server server = new Server();

        public delegate void delPassData(string str, string Text, string port);
        public delegate void tranPassData(String port);

        public List<int> listCode = new List<int>();

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        public class UserInfo
        {
            [BsonId]
            public Guid Id { get; set; }
            public string UserName { get; set; }
            public bool Admin { get; set; }
        }
        public class Code
        {
            public string code { get; set; }
            public string port { get; set; }
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
        public MongoGRUD db;
        private void Create_Load(object sender, EventArgs e)
        {
            db = new MongoGRUD("whiteboard"); //call Server
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Random rd = new Random();
            int index = rd.Next(0, 9);
            //check if the room existed
            while (listCode.Contains(index))
            {
                index = rd.Next(0, 9);
            }
            listCode.Add(index);
            //create room
            MyRoom i = List_room[index];
            tranPassData tran = new tranPassData(server.tranData);
            tran(i.Port);
            delPassData del = new delPassData(workSpace.funData);
            del(i.Code, textBox1.Text, i.Port);
            //add record
            db.InsertRecords("User", new UserInfo { Admin = true, UserName = textBox1.Text });
            db.InsertRecords("Room", new Code { code = i.Code, port = i.Port });
            this.Close();
            workSpace.Show();
        }
    }
}

