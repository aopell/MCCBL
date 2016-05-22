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
    public partial class SwitchDial : UserControl
    {
        public SwitchDial()
        {
            InitializeComponent();
        }

        public static bool shouldSwitch = new bool();

        private void cancel2_Click(object sender, System.EventArgs e)
        {
            shouldSwitch = false;
        }
        private void ok2_Click(object sender, System.EventArgs e)
        {
            shouldSwitch = true;
        }
    }
}
