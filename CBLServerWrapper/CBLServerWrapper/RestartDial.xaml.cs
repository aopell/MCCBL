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
    public partial class RestartDial : UserControl
    {
        public RestartDial()
        {
            InitializeComponent();
        }
        public static bool shouldRestsrt = new bool();

        private void cancel1_Click(object sender, System.EventArgs e)
        {
            shouldRestsrt = false;
        }
        private void ok1_Click(object sender, System.EventArgs e)
        {
            shouldRestsrt = true;
        }
    }
}
