using CBLServerWrapper.Properties;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool forceClose = false;
        public static string TextToAdd = "";
        OpenFileDialog ChooseFileDialog = new OpenFileDialog();
        System.Windows.Forms.FolderBrowserDialog ChooseFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
        SolidColorBrush textColor;

        /// <summary>
        /// Sends the specified command to the server
        /// </summary>
        /// <param name="command">The exact command text to send</param>
        public void SendCommand(string command)
        {
            ServerManager.MinecraftServer.StandardInput.WriteLine(command);
            ServerManager.MinecraftServer.StandardInput.FlushAsync();
            string timestring = DateTime.Now.ToString("M/d HH:mm:ss");
            ConsoleWindow.AppendColoredText(string.Format("\r[{0}]: {1}", timestring, command), new SolidColorBrush(Color.FromRgb(62, 80, 180)));
            ConsoleWindow.ScrollToEnd();
        }

        /// <summary>
        /// Prepares for server start, then starts the server
        /// </summary>
        private void StartServer()
        {
            ChooseFileDialog.Filter = "Minecraft Server Files (*.jar)|*.jar";
            if (ServerManager.Restart || ChooseFileDialog.ShowDialog() == true)
            {
                ConsoleWindow.AppendText("Loading...");
                ConsoleWindow.ScrollToEnd();

                //Server starts here
                ServerManager.StartServer(ChooseFileDialog.FileName, this);
                label.Content = "Players - 0";
                listBox1.Items.Clear();
                serverStart.IsEnabled = false;
                toolsMenu.IsEnabled = true;
                toolsImport.IsEnabled = true;
                toolsBatch.IsEnabled = true;
                serverStop.IsEnabled = true;
                serverRestart.IsEnabled = true;
                serverSwitch.IsEnabled = true;
            }
            ServerManager.Restart = false;
            ServerManager.Switch = false;
        }

        /// <summary>
        /// Called when the server is closed
        /// </summary>
        public void ServerExited()
        {
            try
            {
                Dispatcher.Invoke(delegate
                {
                    ConsoleWindow.AppendColoredText("\rStopped\r", new SolidColorBrush(Color.FromRgb(244, 67, 54)));
                    ConsoleWindow.ScrollToEnd();

                    listBox1.Items.Clear();
                    label.Content = "Server Offline";

                    serverStart.IsEnabled = true;
                    toolsMenu.IsEnabled = false;
                    toolsImport.IsEnabled = false;
                    toolsBatch.IsEnabled = false;
                    serverStop.IsEnabled = false;
                    serverRestart.IsEnabled = false;
                    serverSwitch.IsEnabled = false;

                    ServerManager.MinecraftServer.StandardInput.Flush();
                    if (ServerManager.Restart || ServerManager.Switch)
                    {
                        StartServer();
                        ServerManager.Restart = false;
                        ServerManager.Switch = false;
                    }
                });
            }
            catch
            {

            }
        }

        /// <summary>
        /// Adds text to the ConsoleWindow
        /// </summary>
        public void UpdateText()
        {
            string timestring = DateTime.Now.ToString("M/d HH:mm:ss");
            ConsoleWindow.AppendColoredText(string.Format("\r[{0}]: {1}", timestring, TextToAdd), new SolidColorBrush(Color.FromRgb(62, 80, 180)));
            ConsoleWindow.ScrollToEnd();
        }

        /// <summary>
        /// Called when most status messages are recieved from the server
        /// </summary>
        /// <param name="recievedMessage">The recieved message</param>
        public void OutputDataRecieved(string recievedMessage)
        {
            ServerManager.LastRecievedMessage = recievedMessage;

            try
            {
                Dispatcher.Invoke(delegate
                {
                    //Checks if message is not blank or null
                    if (ServerManager.LastRecievedMessage != null && ServerManager.LastRecievedMessage.Trim() != "")
                    {
                        //Checks for players logging in so text can be yellow
                        if (ServerManager.LastRecievedMessage.ContainsAny(new string[] { "logged in with entity id", "UUID of player", "left the game", "lost connection:", "joined the game" }))
                        {
                            textColor = new SolidColorBrush(Color.FromRgb(254, 159, 0));
                            if (ServerManager.LastRecievedMessage.Contains("logged in with entity id"))
                            {
                                string timeString = DateTime.Now.ToString("M/d HH:mm:ss");
                                ServerManager.ChatHistory.Add(ChatTools.FilterCommand(string.Format("\r[{0}]: {1}", timeString, ChatTools.FilterCommand(ServerManager.LastRecievedMessage))));

                                string player = ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Split('[')[0].Trim();
                                ServerManager.LoggedInPlayers.Add(player);
                                ListBoxItem item = new ListBoxItem();
                                item.Content = player;
                                item.MouseLeftButtonUp += I_MouseLeftButtonUp;
                                item.FontSize = 12;
                                item.Height = 30;
                                listBox1.Items.Add(item);
                                label.Content = "Players - " + listBox1.Items.Count;

                                if (File.Exists(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + "\\motd.mccbl"))
                                {
                                    SendCommand(ChatTools.Tellraw("@a[score_motd_min=1]", TellrawColor.white, "Recent Chat History:"));
                                    for (int i = 11; i > 0; i--)
                                    {
                                        try
                                        {
                                            SendCommand(ChatTools.Tellraw("@a[score_motd_min=1]", TellrawColor.white, ServerManager.ChatHistory[ServerManager.ChatHistory.Count - i].Replace(Environment.NewLine, "")));
                                        }
                                        catch (ArgumentOutOfRangeException)
                                        {

                                        }
                                    }
                                    SendCommand(ChatTools.Tellraw("@a[score_motd_min=1]", TellrawColor.white, "--------------------"));
                                    CBLFile motdFile = new CBLInterpreter(this, "MOTD").Interpret(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + "\\motd.mccbl");
                                    motdFile.Execute(player, "");
                                }
                            }
                            else if (ServerManager.LastRecievedMessage.Contains("lost connection"))
                            {
                                string timeString = DateTime.Now.ToString("M/d HH:mm:ss");
                                ServerManager.ChatHistory.Add(ChatTools.FilterCommand(string.Format("\r[{0}]: {1}", timeString, ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Trim())));

                                string player = ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Split(new string[] { "lost connection" }, StringSplitOptions.None)[0].Trim();
                                ServerManager.LoggedInPlayers.Remove(player);
                                foreach (object item in listBox1.Items)
                                {
                                    try
                                    {
                                        if (((ListBoxItem)item).Content.ToString() == player)
                                        {
                                            listBox1.Items.Remove(item);
                                        }
                                    }
                                    catch { }
                                }
                                label.Content = "Players - " + listBox1.Items.Count;
                            }
                        }
                        //Checks for events that make green text
                        else if (ServerManager.LastRecievedMessage.ContainsAny(new string[] { "Done (", "Starting minecraft server", "Starting Minecraft server", "has just earned the achievement" }))
                        {
                            textColor = new SolidColorBrush(Color.FromRgb(76, 175, 80));
                        }
                        //Checks for errors sent through the standard output and colors them red
                        else if (ServerManager.LastRecievedMessage.ContainsAny(new string[] { "moved too quickly!", "with pending removal and duplicate UUID", "change, or is the server overloaded?", "is sending move packets too frequently", "exception was", "FAILED TO BIND", "perhaps a server", "stopping server", "Stopping server", "Stopping the server" }))
                        {
                            textColor = new SolidColorBrush(Color.FromRgb(244, 67, 54)); ;
                        }
                        //Finds FATAL errors sent through standard output and colors them orange
                        else if (ServerManager.LastRecievedMessage.ContainsAny(new string[] { "A single server tick took", "server will forcibly shutdown", "This crash report has" }))
                        {
                            textColor = new SolidColorBrush(Color.FromRgb(254, 151, 0));
                        }
                        //Checks if the message is a chat message
                        if (ChatTools.FilterCommand(ServerManager.LastRecievedMessage).StartsWith("<"))
                        {
                            string timeString = DateTime.Now.ToString("M/d HH:mm:ss");
                            //Adds the message to the chat history
                            ServerManager.ChatHistory.Add(ChatTools.FilterCommand(string.Format("\r[{0}]: {1}", timeString, ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Trim())));
                            textColor = new SolidColorBrush(Color.FromRgb(155, 38, 175));
                            //Finds the actual chat message portion of the incoming message
                            string chatMessage = ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Split('>')[1];
                            //Gets the username of the player who sent the message
                            string user = ChatTools.FilterUsername(ChatTools.FilterCommand(ServerManager.LastRecievedMessage));
                            //Checks for custom commands
                            bool wasUserMade = false;
                            if (chatMessage.TrimStart().StartsWith("@!"))
                            {
                                Dictionary<string, CBLFile> CustomCommands = new Dictionary<string, CBLFile>();

                                string cmdDir = ServerManager.MinecraftServer.StartInfo.WorkingDirectory + "\\commands";
                                if (Directory.Exists(cmdDir))
                                {
                                    foreach (string file in Directory.GetFileSystemEntries(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + "\\commands"))
                                    {
                                        CustomCommands.Add(System.IO.Path.GetFileName(file), new CBLInterpreter(this, System.IO.Path.GetFileName(file)).Interpret(file));
                                    }
                                }

                                //Checks if the message matches a user-made custom command
                                if (CustomCommands.ContainsKey(chatMessage.TrimStart().Split(' ')[0] + ".mccbl"))
                                {
                                    wasUserMade = true;

                                    CustomCommands[chatMessage.TrimStart().Split(' ')[0] + ".mccbl"].Execute(user, chatMessage.TrimStart().Split(' ').Length > 1 ? string.Join(" ", chatMessage.TrimStart().Split(' ').Skip(1)) : "");
                                }

                                //Checks for premade custom commands
                                //TODO: Allow turning these commands on or off
                                //TODO: Specify which players can use these commands

                                //Imports a MCCBL from the given link or file path at the player who used the command
                                //Examples:
                                //@!import http://example.com/mccblfile.mccbl
                                //@!import http://example.com/textfile.txt
                                //@!import C:\Users\TestUser\Documents\localfile.mccbl
                                if (chatMessage.TrimStart().StartsWith("@!import "))
                                {
                                    string link = chatMessage.CommandArgument("@!import");
                                    CBLInterpreter interpreter = new CBLInterpreter(this, link, true, ChatTools.FilterUsername(ChatTools.FilterCommand(ServerManager.LastRecievedMessage)));
                                    ConsoleWindow.AppendColoredText("\rRemote Importing from " + link, new SolidColorBrush(Color.FromRgb(62, 80, 180)));
                                    SendCommand(ChatTools.MultiTellraw("@a", new TellrawColor[] { TellrawColor.gold, TellrawColor.aqua, TellrawColor.gold, TellrawColor.aqua }, new string[] { "Remote Importing from ", link, " as ", "Remote Import.mccbl" }));
                                    //SendCommand("say §6Remote §6Importing §6from §b§l" + link + " §6as §b§lRemote §b§lImport.mccbl");
                                    CBLFile importer = interpreter.Interpret(link);
                                    if (importer != null && importer.Import(this))
                                    {
                                        ConsoleWindow.AppendColoredText("\rSuccessfully imported " + interpreter.commands.Count + " commands", new SolidColorBrush(Color.FromRgb(62, 80, 180)));
                                        SendCommand(ChatTools.MultiTellraw("@a", new TellrawColor[] { TellrawColor.green, TellrawColor.yellow, TellrawColor.green }, new string[] { "Successfully imported ", interpreter.commands.Count.ToString(), " commands" }));
                                        //SendCommand("say §aSuccessfully §aimported §e§l" + interpreter.commands.Count + " §acommands");
                                        SendCommand(ChatTools.Tellraw("@a", TellrawColor.red, "Don't forget to enable the first Command Block if necessary"));
                                        //SendCommand("say §c§l§nDon't §c§l§nforget §cto §cenable §cthe §cfirst §cCommand §cBlock §cif §cnecessary");
                                    }
                                }
                                //Reimports the last locally imported file at the given selector
                                //Does not reimport from a link
                                //Example:
                                //@!reimport @p
                                else if (chatMessage.TrimStart().StartsWith("@!reimport "))
                                {
                                    ImportFile(true, chatMessage.CommandArgument("@!reimport"));
                                }
                                //Restarts the server by sending the 'stop' command and then restarting it
                                //Example:
                                //@!restart
                                else if (chatMessage.TrimStart().StartsWith("@!restart"))
                                {
                                    ServerManager.Restart = true;
                                    Close();
                                }
                                //Adds the specefied text to a text file stored in the server's root directory
                                //Useful to store coordinates or ideas
                                //Example:
                                //@!addline This text will be put in a text file
                                //@!addtext This text will be put in a text file
                                else if (chatMessage.TrimStart().StartsWith("@!addtext ") || chatMessage.TrimStart().StartsWith("@!addline "))
                                {
                                    string textLine = chatMessage.CommandArgument("@!addtext");
                                    timeString = DateTime.Now.ToString("M/d HH:mm:ss");
                                    try
                                    {
                                        File.AppendAllLines(System.IO.Path.GetDirectoryName(ServerManager.ServerJarPath) + ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\')[ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\').Length - 1] + ".txt", new string[] { "[" + timeString + "] <" + user + "> " + textLine });
                                        SendCommand(ChatTools.Tellraw(user, TellrawColor.green, "Added line to file successfully"));
                                    }
                                    catch (Exception ex)
                                    {
                                        SendCommand(ChatTools.Tellraw(user, TellrawColor.red, "Failed to add line to file" + ex.Message));
                                    }
                                }
                                //Views a page (5 lines) from the text file
                                //Example:
                                //@!viewtext 3
                                //@!viewtext
                                else if (chatMessage.TrimStart().StartsWith("@!viewtext "))
                                {
                                    try
                                    {
                                        int page = 0;
                                        if (!int.TryParse(chatMessage.CommandArgument("@!viewtext"), out page))
                                        {
                                            page = 1;
                                        }
                                        string[] fileLines = File.ReadAllLines(System.IO.Path.GetDirectoryName(ServerManager.ServerJarPath) + ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\')[ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\').Length - 1] + ".txt");
                                        SendCommand(ChatTools.Tellraw(user, TellrawColor.aqua, "Page " + page + "/" + Math.Ceiling(fileLines.Length / 5f)));
                                        for (int i = 5 * page - 5; i < 5 * page; i++)
                                        {
                                            try
                                            {
                                                SendCommand(ChatTools.Tellraw(user, TellrawColor.aqua, i + 1 + ": " + fileLines[i]));
                                            }
                                            catch
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        SendCommand(ChatTools.Tellraw(user, TellrawColor.red, "Failed to read file: " + ex.Message));
                                    }
                                }
                                //Deletes the specefied line from the text file
                                //Example:
                                //@!delline 2
                                else if (chatMessage.TrimStart().StartsWith("@!delline "))
                                {
                                    try
                                    {
                                        int line = Convert.ToInt32(chatMessage.CommandArgument("@!delline")) - 1;
                                        string path = System.IO.Path.GetDirectoryName(ServerManager.ServerJarPath) + ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\')[ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\').Length - 1] + ".txt";
                                        List<string> fileLines = File.ReadAllLines(path).ToList();
                                        fileLines.RemoveAt(line);
                                        File.WriteAllLines(path, fileLines);
                                        SendCommand(ChatTools.Tellraw(user, TellrawColor.green, "Successfully deleted line " + line));
                                    }
                                    catch (Exception ex)
                                    {
                                        SendCommand(ChatTools.Tellraw(user, TellrawColor.red, "Failed to delete line: " + ex.Message));
                                    }
                                }
                                //Shows all commands, their syntax, and their description
                                //TODO: Allow user-made commands to have a help message
                                //Example:
                                //@!help
                                //TODO: @!help import
                                else if (chatMessage.TrimStart().StartsWith("@!help"))
                                {
                                    SendCommand(ChatTools.Tellraw(user, TellrawColor.yellow, "@!import [link|filepath]: Imports a remote MCCBL File"));
                                    SendCommand(ChatTools.Tellraw(user, TellrawColor.yellow, "@!reimport [selector]: Reimports last locally imported file (no links)"));
                                    SendCommand(ChatTools.Tellraw(user, TellrawColor.yellow, "@!restart: Restarts the server"));
                                    SendCommand(ChatTools.Tellraw(user, TellrawColor.yellow, "@!addtext | @!addline [text]: Adds a line of text to the server text file"));
                                    SendCommand(ChatTools.Tellraw(user, TellrawColor.yellow, "@!viewtext [page]: Shows a page from the server text file"));
                                    SendCommand(ChatTools.Tellraw(user, TellrawColor.yellow, "@!delline [line number]: Deletes a line of text from the server text file"));

                                    foreach (CBLFile customCommand in CustomCommands.Values)
                                    {
                                        string helpText = customCommand.Properties.ContainsKey("HELP") ? customCommand.Properties["HELP"] : "No help text found";
                                        string syntax = customCommand.Properties.ContainsKey("SYNTAX") ? customCommand.Properties["SYNTAX"] : "";
                                        SendCommand(ChatTools.Tellraw(user, TellrawColor.yellow, customCommand.FileName.Replace(".mccbl", "") + " " + syntax + ": " + helpText));
                                    }
                                }
                                //If no command was found, tell user it doesn't exist
                                else if (!wasUserMade)
                                {
                                    SendCommand(ChatTools.Tellraw(user, TellrawColor.red, "[ERROR] Unknown Command or Bad Syntax, Type @!help for help"));
                                }
                            }

                            //Add chat messages to server console window
                            string timestringa = DateTime.Now.ToString("M/d HH:mm:ss");
                            ConsoleWindow.AppendColoredText(string.Format("\r[{0}]: <{2}> {1}", timestringa, chatMessage.Trim(), ChatTools.FilterUsername(ChatTools.FilterCommand(ServerManager.LastRecievedMessage))), new SolidColorBrush(Color.FromRgb(155, 38, 175)));
                            ConsoleWindow.ScrollToEnd();
                            return;
                        }
                        //Add other messages to server console window
                        string timestring = DateTime.Now.ToString("M/d HH:mm:ss");
                        ConsoleWindow.AppendColoredText(string.Format("\r[{0}]: {1}", timestring, ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Trim()), textColor);
                        ConsoleWindow.ScrollToEnd();
                    }
                });
            }
            catch
            {

            }
        }

        private void I_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string user = ((ListBoxItem)sender).Content.ToString();
                PlayerControls pc = new PlayerControls(user, this);
                pc.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Called when error data is recieved from the server
        /// However, most error data is sent through standard output
        /// </summary>
        /// <param name="recievedCommand">The recieved error message</param>
        public void ErrorDataRecieved(string recievedCommand)
        {
            ServerManager.LastRecievedMessage = recievedCommand;

            try
            {
                Dispatcher.Invoke(delegate
                {
                    if (ServerManager.LastRecievedMessage != null && ServerManager.LastRecievedMessage.Trim() != "")
                    {
                        string timestring = DateTime.Now.ToString("M/d HH:mm:ss");
                        ConsoleWindow.AppendColoredText(string.Format("\r[{0}]: {1}", timestring, ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Trim()), new SolidColorBrush(Color.FromRgb(244, 67, 54)));
                        ConsoleWindow.ScrollToEnd();
                    }
                });
            }
            catch
            {

            }
        }

        /// <summary>
        /// Activates when you send a command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendCommandButton_Click(object sender, RoutedEventArgs e)
        {
            if (CommandInput.Text.Trim() != "" && ServerManager.MinecraftServer != null)
            {
                ServerManager.HistoryIndex = -1;
                ServerManager.MinecraftServer.StandardInput.WriteLine(CommandInput.Text);
                ServerManager.MinecraftServer.StandardInput.FlushAsync();
                ServerManager.SentCommandsList.Add(CommandInput.Text);
                string timestring = DateTime.Now.ToString("M/d HH:mm:ss");
                ConsoleWindow.AppendColoredText(string.Format("\r[{0}]: {1}", timestring, CommandInput.Text), new SolidColorBrush(Color.FromRgb(62, 80, 180)));
                ConsoleWindow.ScrollToEnd();
                CommandInput.Text = "";
            }
        }

        /// <summary>
        /// Activates when the window is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Cancels closing the window if server is restarting or switching
            if (ServerManager.Switch || ServerManager.Restart)
            {
                e.Cancel = true;
                SendCommand("stop");
            }
            //Stops a currently running server and cancels closing
            else if (!forceClose && ServerManager.MinecraftServer != null)
            {
                e.Cancel = true;

                try
                {
                    ServerManager.MinecraftServer.StandardInput.WriteLine("stop");
                    ServerManager.MinecraftServer.StandardInput.Flush();
                }
                catch { }
            }

            //Closes if no server is running
            if (ServerManager.MinecraftServer == null || ServerManager.MinecraftServer.HasExited)
            {
                e.Cancel = false;
            }
        }

        /// <summary>
        /// Used to navigate through previously sent commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (ServerManager.HistoryIndex < ServerManager.SentCommandsList.Count - 1)
                {
                    ServerManager.HistoryIndex++;
                    CommandInput.Text = ServerManager.SentCommandsList[ServerManager.SentCommandsList.Count - ServerManager.HistoryIndex - 1];
                    CommandInput.SelectionStart = CommandInput.Text.Length;
                    CommandInput.SelectionLength = 0;
                }
            }
            else if (e.Key == Key.Down)
            {
                if (ServerManager.HistoryIndex > 0)
                {
                    ServerManager.HistoryIndex--;
                    CommandInput.Text = ServerManager.SentCommandsList[ServerManager.SentCommandsList.Count - ServerManager.HistoryIndex - 1];
                    CommandInput.SelectionStart = CommandInput.Text.Length;
                    CommandInput.SelectionLength = 0;
                }
                else if (ServerManager.HistoryIndex == 0)
                {
                    ServerManager.HistoryIndex = -1;
                    CommandInput.Text = "";
                }
            }
        }

        /// <summary>
        /// Imports a MCCBL file
        /// </summary>
        /// <param name="reimport">Whether or not to reimport the last import</param>
        /// <param name="selector">Selector at which to import</param>
        private void ImportFile(bool reimport = false, string selector = null)
        {
            if (!reimport)
            {
                //Opens a file dialog and then imports the selected file
                ChooseFileDialog.Filter = "CommandBlock Language Files (*.mccbl)|*.mccbl";
                if (ChooseFileDialog.ShowDialog() == true)
                {
                    CBLInterpreter interpreter = new CBLInterpreter(this, ChooseFileDialog.SafeFileName);
                    ConsoleWindow.AppendText("Importing " + ChooseFileDialog.SafeFileName);
                    SendCommand(ChatTools.MultiTellraw("@a", new TellrawColor[] { TellrawColor.gold, TellrawColor.aqua }, new string[] { "Importing ", ChooseFileDialog.SafeFileName }));
                    //SendCommand("say §6Importing §b§l" + ChooseFileDialog.SafeFileName);
                    CBLFile importer = interpreter.Interpret(ChooseFileDialog.FileName);
                    if (importer != null && importer.Import(this))
                    {
                        SendCommand(ChatTools.MultiTellraw("@a", new TellrawColor[] { TellrawColor.green, TellrawColor.yellow, TellrawColor.green }, new string[] { "Successfully imported ", importer.Commands.Count.ToString(), " commands" }));
                        ConsoleWindow.AppendText("Successfully imported " + importer.Commands.Count + " commands");
                        SendCommand(ChatTools.Tellraw("@a", TellrawColor.red, "Don't forget to enable the first Command Block if necessary"));
                        //SendCommand("say §aSuccessfully §aimported §e§l" + importer.Commands.Count + " §acommands");
                        //SendCommand("say §c§l§nDon't §c§l§nforget §cto §cenable §cthe §cfirst §cCommand §cBlock §cif §cnecessary");
                    }
                    else
                    {
                        SendCommand(ChatTools.Tellraw("@a", TellrawColor.red, "[ERROR] Import was cancelled or failed"));
                    }
                }
                else
                {
                    SendCommand(ChatTools.Tellraw("@a", TellrawColor.red, "[ERROR] Import was cancelled or failed"));
                }
            }
            else
            {
                //Reimports the previously imported commands
                if (ChooseFileDialog.SafeFileName.EndsWith(".mccbl"))
                {
                    CBLInterpreter interpreter = new CBLInterpreter(this, ChooseFileDialog.SafeFileName, false, selector ?? Settings.Default.LastSelector);
                    ConsoleWindow.AppendText("Reimporting " + ChooseFileDialog.SafeFileName);
                    SendCommand(ChatTools.MultiTellraw("@a", new TellrawColor[] { TellrawColor.gold, TellrawColor.aqua }, new string[] { "Reimporting ", ChooseFileDialog.SafeFileName }));
                    //SendCommand("say §6Reimporting §b§l" + ChooseFileDialog.SafeFileName);
                    CBLFile importer = interpreter.Interpret(ChooseFileDialog.FileName);
                    if (importer != null && importer.Import(this))
                    {
                        ConsoleWindow.AppendText("Successfully imported " + importer.Commands.Count + " commands");
                        SendCommand(ChatTools.MultiTellraw("@a", new TellrawColor[] { TellrawColor.green, TellrawColor.yellow, TellrawColor.green }, new string[] { "Successfully imported ", importer.Commands.Count.ToString(), " commands" }));
                        SendCommand(ChatTools.Tellraw("@a", TellrawColor.red, "Don't forget to enable the first Command Block if necessary"));
                        //SendCommand("say §aSuccessfully §areimported §e§l" + importer.Commands.Count + " §acommands");
                        //SendCommand("say §c§l§nDon't §c§l§nforget §cto §cenable §cthe §cfirst §cCommand §cBlock §cif §cnecessary");
                    }
                    else
                    {
                        SendCommand(ChatTools.Tellraw("@a", TellrawColor.red, "[ERROR] Reimport was cancelled or failed"));
                    }
                }
                else
                {
                    SendCommand(ChatTools.MultiTellraw("@a", new TellrawColor[] { TellrawColor.red, TellrawColor.aqua, TellrawColor.red }, new string[] { "Reimporting ", ChooseFileDialog.SafeFileName, " Failed!" }));
                    //SendCommand("say §cReimporting §b§l" + ChooseFileDialog.SafeFileName + " §cFailed!");
                }
            }
        }

        /// <summary>
        /// Activates when you click the stop server button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serverStop_Click(object sender, RoutedEventArgs e)
        {
            if (serverStop.IsEnabled && MessageBox.Show("Are you sure you want to stop the server?", "Stop Server?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ServerManager.Restart = false;
                ServerManager.Switch = false;
                Close();
                serverStart.IsEnabled = true;
                serverStop.IsEnabled = false;
                serverRestart.IsEnabled = false;
                serverSwitch.IsEnabled = false;
                toolsMenu.IsEnabled = false;
            }
        }

        /// <summary>
        /// Activates when you click the start server button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serverStart_Click(object sender, RoutedEventArgs e)
        {
            if (serverStart.IsEnabled)
            {
                StartServer();
            }
        }

        /// <summary>
        /// Opens the OPs file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolsOperators_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + @"\ops.json");
        }

        /// <summary>
        /// Opens the server.properties file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolsProperties_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + @"\server.properties");
        }

        /// <summary>
        /// Opens the whitelist.json file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolsWhitelist_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + @"\whitelist.json");
        }

        /// <summary>
        /// Saves the set min RAM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minRAM_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.Default.MinRAM = minRAM.Text;
            Settings.Default.Save();
        }

        /// <summary>
        /// Saves the set max RAM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maxRAM_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.Default.MaxRAM = maxRAM.Text;
            Settings.Default.Save();
        }

        /// <summary>
        /// Activates on startup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            minRAM.Text = Settings.Default.MinRAM;
            maxRAM.Text = Settings.Default.MaxRAM;
        }

        /// <summary>
        /// Shows the about dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void helpAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutDialog dialog = new AboutDialog();
            await DialogHost.Show(dialog, "ADialogBox");
            //(new AboutDialog()).ShowDialog();
        }

        /// <summary>
        /// Activates when clicking the import commands menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolsImport_Click(object sender, RoutedEventArgs e)
        {
            if (toolsImport.IsEnabled)
            {
                ImportFile();
            }
        }

        /// <summary>
        /// Activates when clicking the restart menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serverRestart_Click(object sender, RoutedEventArgs e)
        {
            if (serverRestart.IsEnabled && MessageBox.Show("Are you sure you want to restart?", "Confirm Restart", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ServerManager.Restart = true;
                Close();
            }
        }

        /// <summary>
        /// Activates when clicking the switch menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serverSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (serverSwitch.IsEnabled && MessageBox.Show("Are you sure you want to switch servers?", "Confirm Switch Server", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ServerManager.Switch = true;
                Close();
            }
        }

        /// <summary>
        /// Activates when clicking the help menu item
        /// Launches help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpHelp_Click(object sender, RoutedEventArgs e)
        {
            (new HelpWindow()).Show();
        }

        /// <summary>
        /// Activates when clicking the server option menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsOptions_Click(object sender, RoutedEventArgs e)
        {
            //(new ServerSettingsWindow()).Show();
            MessageBox.Show("Not Yet Implemented - Working on it!");
        }

        private void toolsBatch_Click(object sender, RoutedEventArgs e)
        {
            if (toolsBatch.IsEnabled)
            {
                //Opens a folder dialog and then imports all files in the selected folder
                if (ChooseFolderDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                {
                    string directory = ChooseFolderDialog.SelectedPath;
                    foreach (string file in Directory.GetFiles(directory))
                    {
                        if (file.EndsWith(".mccbl"))
                        {
                            string fileName = System.IO.Path.GetFileName(file);
                            CBLInterpreter interpreter = new CBLInterpreter(this, fileName, false, "@e[type=ArmorStand,name=" + System.IO.Path.GetFileName(file).Replace(' ', '_').Trim() + "]");
                            ConsoleWindow.AppendText("Importing " + fileName);
                            SendCommand(ChatTools.MultiTellraw("@a", new TellrawColor[] { TellrawColor.gold, TellrawColor.aqua }, new string[] { "Importing ", fileName }));
                            CBLFile importer = interpreter.Interpret(file);
                            if (importer != null && importer.Import(this))
                            {
                                SendCommand(ChatTools.MultiTellraw("@a", new TellrawColor[] { TellrawColor.green, TellrawColor.yellow, TellrawColor.green }, new string[] { "Successfully imported ", importer.Commands.Count.ToString(), " commands" }));
                                ConsoleWindow.AppendText("Successfully imported " + importer.Commands.Count + " commands");
                                SendCommand(ChatTools.Tellraw("@a", TellrawColor.red, "Don't forget to enable the first Command Block if necessary"));
                            }
                            else
                            {
                                SendCommand(ChatTools.Tellraw("@a", TellrawColor.red, "[ERROR] Import was cancelled or failed for file " + file));
                            }
                        }
                    }

                }
                else
                {
                    SendCommand(ChatTools.Tellraw("@a", TellrawColor.red, "[ERROR] Import was cancelled or failed"));
                }
            }
        }
    }
}
