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

        private void cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShouldRestart = false;
        }

        private void ok_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShouldRestart = true;
        }
    }
}
