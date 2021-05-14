using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace iskolacsengo
{
    public partial class Form1 : Form
    {
        public string dbfilepath = "default.db";
        string textfilepath = null;
        bool dbfileselected = false;
        SQLiteCommand dbreadcommand;
        SQLiteCommand dbwritecommand;
        DataTable dt2 = new DataTable(); // txt processing
        List<sbyte> startTimeHR = new List<sbyte>();
        List<sbyte> startTimeMM = new List<sbyte>();
        List<sbyte> endTimeHR = new List<sbyte>();
        List<sbyte> endTimeMM = new List<sbyte>();
        public Form1()
        {
            InitializeComponent();
            
        }

        private void actualTime_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            actualTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //label1.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "(mp3,wav,mp4,mov,wmv,mpg)|*.mp3;*.wav;*.mp4;*.mov;*.wmv;*.mpg|all files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                axWindowsMediaPlayer1.URL = openFileDialog1.FileName;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = openFileDialog1.FileName;
        }

        private void drawSchedule()
        {
            dbconnector sqliteconn = new dbconnector(dbfilepath);
            sqliteconn.openConnection();
            dbreadcommand = new SQLiteCommand("SELECT * FROM ringschedule", sqliteconn.GetConnection());
            SQLiteDataReader dbreader = dbreadcommand.ExecuteReader();
            sqliteconn.closeConnection();
            DataTable dt = new DataTable();
            dataGridView1.DataSource = dt;
            dt.Load(dbreader);
        }

        private bool updateSchedule(string dbfile)
        {
            bool result = false;
            string dbpath = dbfilepath;
            if (dbfile != "" || dbfile != null) dbpath = dbfile;

            // Change schedule, lets read this thing



            return result;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Import a database file and use it
            openFileDialog2.InitialDirectory = @"C:\";
            openFileDialog2.Title = "Select your SQLITE database file";
            openFileDialog2.DefaultExt = "db";
            openFileDialog2.Filter = "SQLITE DB files (*.db)|*.db|SQLITE files with sqlite extension (*.sqlite)|*.sqlite|All files (*.*)|*.*";
            openFileDialog2.FilterIndex = 1;
            openFileDialog2.ShowDialog();
             if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
             dbfilepath = openFileDialog1.FileName;
         
            }


        }
    }
}
