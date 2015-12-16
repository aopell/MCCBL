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
    public partial class ScriptLoader : Form
    {
        public ScriptLoader(string filename, string[] filelines, List<Command> commands)
        {
            InitializeComponent();
            try
            {
                textBox4.Text = filename;
                textBox1.Text = filelines[2].Replace("@VERSION ", "");
                textBox2.Text = filelines[0].Replace("@TYPE ", "");
                textBox3.Text = filelines[1].Replace("@CHAIN ", "").Replace("ON", "Always Active").Replace("OFF", "Needs Redstone");
                foreach (Command c in commands)
                {
                    richTextBox1.AppendText(string.Format("\r\n{0}: {1}", c.CommandIndex, c.CommandText).Trim());
                }
                richTextBox1.Text = richTextBox1.Text.Trim();
                label1.Text = string.Format("Commands Preview ({0} Commands)", commands.Count);
                textBox5.Text = Properties.Settings.Default.LastSelector;
            }
            catch (Exception e)
            {
                textBox4.Text = "Error Loading Script";
                textBox1.Text = e.GetType().Name;
                textBox2.Text = e.Message;
                richTextBox1.Text = e.StackTrace;
                button1.Enabled = false;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CBLInterpreter.Selector = textBox5.Text;
            Properties.Settings.Default.LastSelector = textBox5.Text;
            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
