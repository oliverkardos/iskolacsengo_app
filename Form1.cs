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
//using System.Threading;
using System.Timers;


namespace iskolacsengo
{

    public partial class Form1 : Form
    {
        public string dbfilepath = "default.db";
        string textfilepath = null;
        string ringtone = "bell.mp3";
        SQLiteCommand dbreadcommand;
      //  System.Timers.Timer timer;
        int LengthOfClassesInMinutes = 45;
        int LengthOfBreaksInMinutes = 10;
        int StartTimeOfFirstClassHour = 8;
        int StartTimeOfFirstClassMinute = 0;
        bool offsetenabled = false;
        System.Windows.Forms.Timer MyTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer MyTimerForBreaks = new System.Windows.Forms.Timer();
        public Form1()
        {
            InitializeComponent();

        }

        private void actualTime_Click(object sender, EventArgs e)
        {

        }

        private void determineFirstBellTime()
        {
            // get actual time
            // StartTimeOfFirstClassHour
            //string currenttime = DateTime.Now.ToString("HH:mm:ss");
            //string currenthour_s = DateTime.Now.ToString("HH");
            //string currentmin_s = DateTime.Now.ToString("mm");
            //int currenthour = Convert.ToInt16(currenthour_s);
            //int currentmin = Convert.ToInt16(currentmin_s);

            // are we on time?
            // determine if we are ahead of first lesson
            DateTime currentTime = DateTime.Now;
            
            if (currentTime.Hour <= StartTimeOfFirstClassHour && !offsetenabled)
            {
                if (currentTime.Minute <= StartTimeOfFirstClassMinute)
                {
                    offsetenabled = true;
                    // we are before the first class
                    // calculate offset
                    int offset = currentTime.Minute - StartTimeOfFirstClassMinute;
                    MyTimer.Interval = (offset * 60 * 1000); // mins
                    MyTimer.Tick += new EventHandler(button3_Click);
                    MyTimer.Start();
                }
            else
                {
                    // regular mode
                    MyTimer.Interval = (LengthOfClassesInMinutes * 60 * 1000); // mins
                    MyTimer.Tick += new EventHandler(button3_Click);
                    MyTimer.Start();

                    int calculatebreakintervals = LengthOfClassesInMinutes + LengthOfBreaksInMinutes;
                    MyTimerForBreaks.Interval = (calculatebreakintervals * 60 * 1000); // mins
                    MyTimerForBreaks.Tick += new EventHandler(button3_Click);
                    MyTimerForBreaks.Start();


                }
            }
           


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            actualTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //label1.Text = DateTime.Now.ToString("HH:mm:ss");
            //timer = new System.Timers.Timer();
            //timer.Interval = 1000;
            //timer.Elapsed += Timer_Elapsed;

        }

        //private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    //  throw new NotImplementedException();
        //    //DateTime currentTime = DateTime.Now;
        //    //DateTime userTime = dateTimePicker.Value;
        //    //if (currentTime.Hour == userTime.Hour)
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            btnPlayRingtone.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;

           // System.Windows.Forms.Timer MyTimer = new System.Windows.Forms.Timer();
            MyTimer.Interval = (1 * 60 * 1000); // 45 mins
            MyTimer.Tick += new EventHandler(button3_Click);
            MyTimer.Start();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "(mp3,wav,mp4,mov,wmv,mpg)|*.mp3;*.wav;*.mp4;*.mov;*.wmv;*.mpg|all files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                axWindowsMediaPlayer1.URL = openFileDialog1.FileName;
            ringtone = openFileDialog1.FileName;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //axWindowsMediaPlayer1.URL = openFileDialog1.FileName;
            axWindowsMediaPlayer1.URL = ringtone;
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


            //foreach (var row in dt.AsEnumerable())
            //{
            //    //   int id = row.Field<int>("ID");
            //    LengthOfClassesInMinutes = Convert.ToInt16(row.Field<int>("LengthOfClassesInMinutes"));
            //    LengthOfBreaksInMinutes = row.Field<int>("LengthOfBreaksInMinutes");
            //    StartTimeOfFirstClassHour = row.Field<int>("StartTimeOfFirstClassHour");
            //    StartTimeOfFirstClassMinute = row.Field<int>("StartTimeOfFirstClassMinute");
            //}

            DataRow[] dr = dt.Select("ID=1");
            var lessons = "";
            var breaks = "";
            var firsthr = "";
            var firstmm="";
            foreach (DataRow row in dr)
            {
                lessons = row["LengthOfClassesInMinutes"].ToString();
                breaks = row["LengthOfBreaksInMinutes"].ToString();
                firsthr = row["StartTimeOfFirstClassHour"].ToString();
                firstmm = row["StartTimeOfFirstClassMinute"].ToString();
            }
            StartTimeOfFirstClassHour = Convert.ToInt16(firsthr);
            StartTimeOfFirstClassMinute = Convert.ToInt16(firstmm);
            determineFirstBellTime();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            drawSchedule();
            determineFirstBellTime();
            button6.Enabled = false;
            button7.Enabled = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyTimer.Stop();
            MyTimerForBreaks.Stop();
            button6.Enabled = true;
            button7.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {

            button6.Enabled = false;
            button7.Enabled = false;
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
          //  dt2.Columns.Add("ID", typeof(int));
            dt2.Columns.Add("LengthOfClassesInMinutes", typeof(int));
            dt2.Columns.Add("LengthOfBreaksInMinutes", typeof(int));
            dt2.Columns.Add("StartTimeOfFirstClassHOUR", typeof(int));
            dt2.Columns.Add("StartTimeOfFirstClassMINUTE", typeof(int));

//            string firstline = sr.ReadLine();

            string linebylineread;
            while ((linebylineread = sr.ReadLine()) != null)
            {
                string[] splitatcomma = linebylineread.Split(';');
                LengthOfClassesInMinutes = Convert.ToInt16(splitatcomma[1]);
                LengthOfBreaksInMinutes = Convert.ToInt16(splitatcomma[2]);
                StartTimeOfFirstClassHour = Convert.ToInt16(splitatcomma[3]);
                StartTimeOfFirstClassMinute = Convert.ToInt16(splitatcomma[4]);
                dt2.Rows.Add(splitatcomma);
                // dt2.Rows.Add(LengthOfClassesInMinutes, LengthOfBreaksInMinutes, StartTimeOfFirstClassHour, StartTimeOfFirstClassMinute);
                //dt2.Rows.Add(new object[] { LengthOfClassesInMinutes, LengthOfBreaksInMinutes, StartTimeOfFirstClassHour, StartTimeOfFirstClassMinute });
                
            }
            dataGridView1.DataSource = dt2;
            sr.Close();
            determineFirstBellTime();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //button2.Enabled = true;
            //btnPlayRingtone.Enabled = false;
            //button4.Enabled = false;
            //button6.Enabled = false;
            //button7.Enabled = false;

            //// System.Windows.Forms.Timer MyTimer = new System.Windows.Forms.Timer();
            //MyTimer.Interval = (45 * 60 * 1000); // 45 mins
            //MyTimer.Tick += new EventHandler(button3_Click);
            //MyTimer.Start();
            determineFirstBellTime(); //this also handles event launches
        }
    }


    }


