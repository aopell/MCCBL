using System.Windows.Controls;

namespace CBLServerWrapper
{
    /// <summary>
    /// Interaction logic for Stop.xaml
    /// </summary>
    public partial class RestartDialog : UserControl
    {
        public RestartDialog()
        {
            InitializeComponent();
        }
        public bool ShouldRestart;

        private void cancel1_Click(object sender, System.EventArgs e)
        {
            ShouldRestart = false;
        }
        private void ok1_Click(object sender, System.EventArgs e)
        {
            ShouldRestart = true;
        }
    }
}
