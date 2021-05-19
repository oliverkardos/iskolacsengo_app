﻿using System;
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
using System.Threading;
using System.Timers;


namespace iskolacsengo
{

    public partial class Form1 : Form
    {
        public string dbfilepath = "default.db";
        string textfilepath = null;
        SQLiteCommand dbreadcommand;
        List<int> ids = new List<int>();
        List<int> starthr = new List<int>();
        List<int> startmm = new List<int>();
        List<int> endhr = new List<int>();
        List<int> endmm = new List<int>();
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
            button2.Enabled = true;
            btnPlayRingtone.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;

            System.Windows.Forms.Timer MyTimer = new System.Windows.Forms.Timer();
            MyTimer.Interval = (45 * 60 * 1000); // 45 mins
            MyTimer.Tick += new EventHandler(button3_Click);
            MyTimer.Start();

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
            dbconnector sqliteconn = new dbconnector("default.db");
            sqliteconn.openConnection();
            dbreadcommand = new SQLiteCommand("SELECT * FROM ringschedule", sqliteconn.GetConnection());
            SQLiteDataReader dbreader = dbreadcommand.ExecuteReader();
            DataTable dt = new DataTable();
            dataGridView1.DataSource = dt;
            dt.Load(dbreader);
            sqliteconn.closeConnection();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            drawSchedule();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

           
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Select your TEXT CSV database file";
            openFileDialog1.DefaultExt = "csv";
            openFileDialog1.Filter = "CSV TEXT files (*.csv)|*.csv|CSV files with txt extension (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.ShowDialog();
            textfilepath = openFileDialog1.FileName;
            StreamReader sr = new StreamReader(textfilepath, true); // auto encoding detection
            DataTable dt2 = new DataTable(); // txt processing
            dt2.Clear(); // just to be safe, in case this isnt the first file
                         // manual fill
                         // first the headers
            dt2.Columns.Add("ID", typeof(int));
            dt2.Columns.Add("StartHour", typeof(int));
            dt2.Columns.Add("StartMin", typeof(int));
            dt2.Columns.Add("EndHour", typeof(int));
            dt2.Columns.Add("EndMin", typeof(int));
           
            string linebylineread;
            while ((linebylineread = sr.ReadLine()) != null)
            {
                // string linebylineread = sr.ReadLine();
                string[] splitatcomma = linebylineread.Split(';');
                ids.Add(Convert.ToInt16(splitatcomma[0]));
                starthr.Add(Convert.ToInt16(splitatcomma[1]));
                startmm.Add(Convert.ToInt16(splitatcomma[2]));
                endhr.Add(Convert.ToInt16(splitatcomma[3]));
                endmm.Add(Convert.ToInt16(splitatcomma[4]));
                dt2.Rows.Add(splitatcomma);
            }

         

            dataGridView1.DataSource = dt2;
            sr.Close();
        }


    }


    }