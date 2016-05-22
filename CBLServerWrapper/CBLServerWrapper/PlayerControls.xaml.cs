using System.Windows;
using System.Windows.Controls;

namespace CBLServerWrapper
{
    /// <summary>
    /// Interaction logic for PlayerControls.xaml
    /// </summary>
    public partial class PlayerControls : Window
    {
        MainWindow Window;
        string Username;

        public PlayerControls(string username, MainWindow window)
        {
            InitializeComponent();
            Window = window;
            Username = username;
            label.Content = Username;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (UIElement u in grid.Children)
            {
                try
                {
                    Button b = (Button)u;
                    if (b.Uid == "REQUIRE")
                    {
                        b.IsEnabled = textBox.Text != "";
                    }
                }
                catch { }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Window.SendCommand($"op {Username}");
            Close();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Window.SendCommand($"deop {Username}");
            Close();
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            Window.SendCommand($"whitelist add {Username}");
            Close();
        }

        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            Window.SendCommand($"whitelist remove {Username}");
            Close();
        }

        private void button_Copy3_Click(object sender, RoutedEventArgs e)
        {
            Window.SendCommand($"kill {Username}");
            Close();
        }

        private void button_Copy4_Click(object sender, RoutedEventArgs e)
        {
            Window.SendCommand($"tell {Username} {textBox.Text}");
            Close();
        }

        private void button_Copy5_Click(object sender, RoutedEventArgs e)
        {
            Window.SendCommand($"execute {Username} ~ ~ ~ {textBox.Text}");
            Close();
        }

        private void button_Copy6_Click(object sender, RoutedEventArgs e)
        {
            Window.SendCommand($"kick {Username} {textBox.Text}");
            Close();
        }

        private void button_Copy7_Click(object sender, RoutedEventArgs e)
        {
            Window.SendCommand($"ban {Username} {textBox.Text}");
            Close();
        }

        private void button_Copy8_Click(object sender, RoutedEventArgs e)
        {
            Window.SendCommand($"banip {Username} {textBox.Text}");
            Close();
        }
    }
}
