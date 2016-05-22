using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CBLServerWrapper
{
    /// <summary>
    /// Interaction logic for Stop.xaml
    /// </summary>
    public partial class StopDial : UserControl
    {
        public StopDial()
        {
            InitializeComponent();
        }

        public static bool shouldStop = new bool();

        private void cancel_Click(object sender, System.EventArgs e)
        {
            shouldStop = false;
        }
        private void ok_Click(object sender, System.EventArgs e)
        {
            shouldStop = true;
        }
    }
}
