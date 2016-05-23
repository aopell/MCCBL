using System.Windows.Controls;

namespace CBLServerWrapper
{
    /// <summary>
    /// Interaction logic for Stop.xaml
    /// </summary>
    public partial class SwitchDialog : UserControl
    {
        public SwitchDialog()
        {
            InitializeComponent();
        }

        public bool ShouldSwitch;

        private void cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShouldSwitch = false;
        }

        private void ok_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShouldSwitch = true;
        }
    }
}
