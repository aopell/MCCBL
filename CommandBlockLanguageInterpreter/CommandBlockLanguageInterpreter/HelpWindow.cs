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
    public partial class HelpWindow : Form
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            label8.Text = Math.Round(new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / 1073741824d, 2) + "GB";
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://minecraft.gamepedia.com/Commands#Summary_of_commands");
        }
    }
}
