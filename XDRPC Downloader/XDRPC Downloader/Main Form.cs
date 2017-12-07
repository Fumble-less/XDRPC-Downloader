using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XDRPC_Downloader
{
    public partial class Form1 : Form
    {
        Utilities util = new Utilities();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = util.Connect("192.168.1.9"); //change this little bastard
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            util.snowy();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = util.CheckForXDRPCInINI().ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            util.AddXDRPCInINI();
        }
    }
}
