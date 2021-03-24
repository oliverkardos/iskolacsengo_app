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
using System.Globalization;

namespace iskolacsengo
{
    public partial class Form1 : Form
    {
        Utemezes sched;
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void idoKezeles()
        {
            while(true)
            {
                DateTime dt = DateTime.Now;
                label1.Text = dt.ToString("HH:mm:ss");
                String currentTimeString = dt.ToString("yyyy:MM:dd:HH:mm").Replace(":","");
                textBox1.Text = currentTimeString;
                Task.Delay(200).Wait();
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void loadConfigurationFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = "";
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
            idoKezeles();
        }

        private void Form1_Validated(object sender, EventArgs e)
        {

        }
    }
}
