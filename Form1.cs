using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Csengo_utemezes;

namespace iskolacsengo
{
    public partial class Form1 : Form
    {
        Utemezes sched;
        public Form1()
        {
            InitializeComponent();
        }

        private void actualTime_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //actualTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void loadConfigurationFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = "";
            FileDialog fd;
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Application.ExecutablePath.ToString();
                ofd.Filter = "txt files (*.txt)|*.txt";
                ofd.FilterIndex = 1;
                //ofd.RestoreDirectory = true;
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    filePath = ofd.FileName;
                }
            }
            sched = new Utemezes(filePath);
        }
    }
}
