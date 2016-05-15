using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandBlockLanguageInterpreter
{
    public partial class PlayerControls : Form
    {
        string Username;
        MainWindow Window;

        public PlayerControls(string username, MainWindow window)
        {
            Username = username;
            Window = window;
            InitializeComponent();
        }

        private void SendCommand(string command)
        {
            Window.SendCommand(command);
        }

        private void PlayerControls_Load(object sender, EventArgs e)
        {
            label1.Text = Username;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendCommand($"op {Username}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SendCommand($"deop {Username}");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SendCommand($"whitelist add {Username}");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SendCommand($"whitelist remove {Username}");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SendCommand($"kill {Username}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendCommand($"tell {Username} {textBox1.Text}");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SendCommand($"execute ~ ~ ~ {Username} {textBox1.Text}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendCommand($"kick {Username} {textBox1.Text}");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendCommand($"ban {Username} {textBox1.Text}");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SendCommand($"banip {Username} {textBox1.Text}");
        }
    }
}
