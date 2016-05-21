using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandBlockLanguageInterpreter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
            catch (Exception e)
            {
                const string crashMessage =
                    "It looks like you found a crash.\nPaste the entire contents of this file into a new Github issue which you can create at https://github.com/aopell/MCCBL/issues/new\n\nThe crash details are listed below:\n";
                MessageBox.Show("An error has occurred. Please check " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MCCBL-CRASH.log\n\n" + e.ToString());
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MCCBL-CRASH.log", crashMessage + "\n\n" + e.ToString() + "\n\n" + (e.InnerException != null ? e.InnerException.ToString() : ""));
            }
        }
    }
}
