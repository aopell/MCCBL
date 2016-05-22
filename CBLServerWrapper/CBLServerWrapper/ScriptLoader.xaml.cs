using CBLServerWrapper.Properties;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CBLServerWrapper
{
    /// <summary>
    /// Interaction logic for ScriptLoader.xaml
    /// </summary>
    public partial class ScriptLoader : Window
    {
        public ScriptLoader(string filename, string[] filelines, List<Command> commands, Dictionary<string, string> atTags)
        {
            InitializeComponent();
            try
            {
                label.Content = filename;
                label3.Content = atTags.ContainsKey("VERSION") ? atTags["VERSION"] : "No Version Supplied";
                label3_Copy.Content = atTags.ContainsKey("TYPE") ? atTags["TYPE"].Replace("REPEAT", "Repeating").Replace("ONCE", "Not Repeating") : "Default: Not Repeating";
                label3_Copy1.Content = atTags.ContainsKey("CHAIN") ? atTags["CHAIN"].Replace("ON", "Always Active").Replace("OFF", "Require Redstone") : "Default: Always Active";
                label1.Content = string.Format("{0} Commands", commands.Count);
                selectorBox.Text = Settings.Default.LastSelector;
            }
            catch (Exception e)
            {
                label.Content = "Error Loading Script";
                label1.Content = e.GetType().Name;
                button.IsEnabled = false;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CBLInterpreter.Selector = selectorBox.Text;
            Settings.Default.LastSelector = selectorBox.Text;
            Settings.Default.Save();
            DialogResult = true;
            Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
