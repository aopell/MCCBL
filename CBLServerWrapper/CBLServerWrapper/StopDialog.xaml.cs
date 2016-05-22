using System.Windows.Controls;

namespace CBLServerWrapper
{
    /// <summary>
    /// Interaction logic for Stop.xaml
    /// </summary>
    public partial class StopDialog : UserControl
    {
        public StopDialog()
        {
            InitializeComponent();
        }

        public bool ShouldStop;

        private void cancel_Click(object sender, System.EventArgs e)
        {
            ShouldStop = false;
        }
        private void ok_Click(object sender, System.EventArgs e)
        {
            ShouldStop = true;
        }
    }
}
