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

        private void cancel2_Click(object sender, System.EventArgs e)
        {
            ShouldSwitch = false;
        }
        private void ok2_Click(object sender, System.EventArgs e)
        {
            ShouldSwitch = true;
        }
    }
}
