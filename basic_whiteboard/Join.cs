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
    public partial class Join : Form
    {
        public Join()
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
        static String FindPort(List<MyRoom> List, String a)
        {
            foreach (MyRoom c in List_room)
            {
                if (c.Code == a)
                    return c.Port;
            }
            return null;
        }
        private WorkSpace ws = new WorkSpace();

        public delegate void delPassData(string str, string Text, string port);

        private void button1_Click(object sender, EventArgs e)
        {
            String port = FindPort(List_room, textBox2.Text);
            if (port != null)
            {
                delPassData del = new delPassData(ws.funData);
                del(textBox2.Text, textBox1.Text, port);
                db.InsertRecords("User", new UserInfo { Admin = false, UserName = textBox1.Text });
                ws.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid code!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class UserInfo
        {
            [BsonId]
            public Guid Id { get; set; }
            public string UserName { get; set; }
            public bool Admin { get; set; }
            public string G_Id { get; set; }
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
        private void Join_Load(object sender, EventArgs e)
        {
            db = new MongoGRUD("WhiteBoardRealtime"); //call Server

        }
    }
}


