﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtMethods;
using System.Text.RegularExpressions;
using System.Net;
using CommandBlockLanguageInterpreter.Properties;

namespace CommandBlockLanguageInterpreter
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool forceClose = false;
        public static string TextToAdd = "";

        /// <summary>
        /// Sends the specified command to the server
        /// </summary>
        /// <param name="command">The exact command text to send</param>
        public void SendCommand(string command)
        {
            ServerManager.MinecraftServer.StandardInput.WriteLine(command);
            ServerManager.MinecraftServer.StandardInput.FlushAsync();
            ConsoleWindow.SelectionColor = Color.Cyan;
            string timestring = DateTime.Now.ToString("M/d HH:mm:ss");
            ConsoleWindow.AppendText(string.Format("\r\n[{0}]: {1}", timestring, command));
            ConsoleWindow.ScrollToCaret();
            ConsoleWindow.SelectionColor = Color.White;
        }

        /// <summary>
        /// Prepares for server start, then starts the server
        /// </summary>
        private void StartServer()
        {
            ChooseFileDialog.Filter = "Minecraft Server Files (*.jar)|*.jar";
            if (ServerManager.Restart || ChooseFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                ConsoleWindow.AppendText("Loading...");

                //Server starts here
                ServerManager.StartServer(ChooseFileDialog.FileName, this);

                startToolStripMenuItem.Disable();
                toolsToolStripMenuItem.Enable();
                importCommandBlockLanguageFileToolStripMenuItem.Enable();
                stopToolStripMenuItem.Enable();
                restartToolStripMenuItem.Enable();
                switchToolStripMenuItem.Enable();
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
                Invoke((MethodInvoker)delegate
                {
                    ConsoleWindow.SelectionColor = Color.Red;
                    ConsoleWindow.AppendText("\r\nStopped");
                    ConsoleWindow.SelectionColor = Color.White;

                    startToolStripMenuItem.Enable();
                    toolsToolStripMenuItem.Enabled = false;
                    importCommandBlockLanguageFileToolStripMenuItem.Disable();
                    stopToolStripMenuItem.Disable();
                    restartToolStripMenuItem.Disable();
                    switchToolStripMenuItem.Disable();

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
            ConsoleWindow.SelectionColor = Color.Cyan;
            string timestring = DateTime.Now.ToString("M/d HH:mm:ss");
            ConsoleWindow.AppendText(string.Format("\r\n[{0}]: {1}", timestring, TextToAdd));
            ConsoleWindow.ScrollToCaret();
            ConsoleWindow.SelectionColor = Color.White;
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
                Invoke((MethodInvoker)delegate
                {
                    //Checks if message is not blank or null
                    if (ServerManager.LastRecievedMessage != null && ServerManager.LastRecievedMessage.Trim() != "")
                    {
                        //Checks for players logging in so text can be yellow
                        if (ServerManager.LastRecievedMessage.ContainsAny(new string[] { "logged in with entity id", "UUID of player", "left the game", "lost connection:", "joined the game" }))
                        {
                            ConsoleWindow.SelectionColor = Color.Yellow;
                            if (ServerManager.LastRecievedMessage.Contains(" joined the game"))
                            {
                                string timeString = DateTime.Now.ToString("M/d HH:mm:ss");
                                ServerManager.ChatHistory.Add(ChatTools.FilterCommand(string.Format("\r\n[{0}]: {1}", timeString, ChatTools.FilterCommand(ServerManager.LastRecievedMessage))));
                                try
                                {
                                    //MOTD Beta
                                    //Checks for the motd.mccbl file in the server directory and runs the file
                                    //Also shows 10 most recent chat messages
                                    //TODO: Remove motd scoreboard requirement
                                    //TODO: Allow turning recent chat / motd On/Off
                                    //TODO: Interpret motd like all other MCCBL files to allow comments, math, etc.
                                    string[] motdCommands = File.ReadAllLines(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + "\\motd.mccbl");
                                    SendCommand("tellraw @a[score_motd_min=1] [\"\",{\"text\":\"Recent Chat History:\", \"color\":\"white\"}]");
                                    for (int i = 11; i > 0; i--)
                                    {
                                        try
                                        {
                                            SendCommand("tellraw @a[score_motd_min=1] [\"\",{\"text\":\"" + ServerManager.ChatHistory[ServerManager.ChatHistory.Count - i].Replace(Environment.NewLine, "") + "\", \"color\":\"white\"}]");
                                        }
                                        catch (ArgumentOutOfRangeException)
                                        {

                                        }
                                    }
                                    SendCommand("tellraw @a[score_motd_min=1] [\"\",{\"text\":\"--------------------\", \"color\":\"white\"}]");
                                    foreach (string command in motdCommands)
                                    {
                                        SendCommand(command);
                                    }
                                }
                                catch (FileNotFoundException)
                                {

                                }
                            }
                            else if (ServerManager.LastRecievedMessage.Contains("left the game"))
                            {
                                string timeString = DateTime.Now.ToString("M/d HH:mm:ss");
                                ServerManager.ChatHistory.Add(ChatTools.FilterCommand(string.Format("\r\n[{0}]: {1}", timeString, ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Trim())));
                            }
                        }
                        //Checks for events that make green text
                        else if (ServerManager.LastRecievedMessage.ContainsAny(new string[] { "Done (", "Starting minecraft server", "Starting Minecraft server", "has just earned the achievement" }))
                        {
                            ConsoleWindow.SelectionColor = Color.Lime;
                        }
                        //Checks for errors sent through the standard output and colors them red
                        else if (ServerManager.LastRecievedMessage.ContainsAny(new string[] { "moved too quickly!", "with pending removal and duplicate UUID", "change, or is the server overloaded?", "is sending move packets too frequently", "exception was", "FAILED TO BIND", "perhaps a server", "stopping server", "Stopping server" }))
                        {
                            ConsoleWindow.SelectionColor = Color.Red;
                        }
                        //Finds FATAL errors sent through standard output and colors them orange
                        else if (ServerManager.LastRecievedMessage.ContainsAny(new string[] { "A single server tick took", "server will forcibly shutdown", "This crash report has" }))
                        {
                            ConsoleWindow.SelectionColor = Color.Orange;
                        }
                        //Checks if the message is a chat message
                        if (ChatTools.FilterCommand(ServerManager.LastRecievedMessage).StartsWith("<"))
                        {
                            string timeString = DateTime.Now.ToString("M/d HH:mm:ss");
                            //Adds the message to the chat history
                            ServerManager.ChatHistory.Add(ChatTools.FilterCommand(string.Format("\r\n[{0}]: {1}", timeString, ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Trim())));
                            ConsoleWindow.SelectionColor = Color.Violet;
                            //Finds the actual chat message portion of the incoming message
                            string chatMessage = ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Split('>')[1];
                            //Gets the username of the player who sent the message
                            string user = ChatTools.FilterUsername(ChatTools.FilterCommand(ServerManager.LastRecievedMessage));
                            //Checks for custom commands
                            bool wasUserMade = false;
                            if (chatMessage.TrimStart().StartsWith("@!"))
                            {

                                //Checks if the message matches a user-made custom command
                                foreach (string file in Directory.GetFileSystemEntries(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + "\\commands"))
                                {
                                    if (chatMessage.TrimStart().Split(' ')[0] + ".mccbl" == Path.GetFileName(file))
                                    {
                                        wasUserMade = true;
                                        MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControl();
                                        sc.Language = "VBScript";
                                        string[] fileLines = File.ReadAllLines(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + "\\commands\\" + Path.GetFileName(file));
                                        foreach (string s in fileLines)
                                        {
                                            //Replaces {arg} with the first argument
                                            //Replaces {user} with the person who used the command
                                            //TODO: Add support for more arguments
                                            string input = s.Replace("{arg}", chatMessage.Split(new string[] { chatMessage.TrimStart().Split(' ')[0] }, StringSplitOptions.None)[1].TrimStart()).Replace("{user}", user).Replace("{arg}", chatMessage.Split(new string[] { chatMessage.TrimStart().Split(' ')[0] }, StringSplitOptions.None)[1].TrimStart());
                                            string output;
                                            //Replaces math expressions in ([ ]) with their answers
                                            if (input.Contains("([") && input.Contains("])"))
                                            {
                                                output = input;
                                                Regex r = new Regex(@"\(\[.+?\]\)");
                                                MatchCollection m = r.Matches(input);
                                                try
                                                {
                                                    for (int i = 0; i < m.Count; i++)
                                                    {
                                                        object result = sc.Eval(m[i].Value.Replace("([", "").Replace("])", ""));
                                                        output = r.Replace(output, result.ToString(), 1);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    output = input;
                                                    SendCommand("tellraw @a [\"\",{\"text\":\"[Error] Failed to interpret math: " + ex.Message + "\",\"color\":\"red\"}]");
                                                }
                                            }
                                            else
                                            {
                                                output = input;
                                            }
                                            SendCommand(output);
                                        }
                                        break;
                                    }
                                }
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
                                string link = chatMessage.Split(new string[] { "@!import" }, StringSplitOptions.None)[1].Trim();
                                CBLInterpreter interpreter = new CBLInterpreter(this, link, true, ChatTools.FilterUsername(ChatTools.FilterCommand(ServerManager.LastRecievedMessage)));
                                ConsoleWindow.SelectionColor = Color.Cyan;
                                ConsoleWindow.AppendText("\nRemote Importing from " + link);
                                SendCommand("say §6Remote §6Importing §6from §b§l" + link + " §6as §b§lRemote §b§lImport.mccbl");
                                interpreter.Interpret(link);
                                ConsoleWindow.SelectionColor = Color.Cyan;
                                ConsoleWindow.AppendText("\nSuccessfully imported " + interpreter.commands.Count + " commands");
                                SendCommand("say §aSuccessfully §aimported §e§l" + interpreter.commands.Count + " §acommands");
                                SendCommand("say §c§l§nDon't §c§l§nforget §cto §cenable §cthe §cfirst §cCommand §cBlock §cif §cnecessary");
                            }
                            //Reimports the last locally imported file at the given selector
                            //Does not reimport from a link
                            //Example:
                            //@!reimport @p
                            else if (chatMessage.TrimStart().StartsWith("@!reimport "))
                            {
                                ImportFile(true, chatMessage.Split(new string[] { "@!reimport" }, StringSplitOptions.None)[1].Trim());
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
                            else if (chatMessage.TrimStart().StartsWith("@!addtext "))
                            {
                                string textLine = chatMessage.Split(new string[] { "@!addtext" }, StringSplitOptions.None)[1].Trim();
                                timeString = DateTime.Now.ToString("M/d HH:mm:ss");
                                try
                                {
                                    File.AppendAllLines(Path.GetDirectoryName(ServerManager.ServerJarPath) + ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\')[ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\').Length - 1] + ".txt", new string[] { "[" + timeString + "] <" + user + "> " + textLine });
                                    SendCommand("say §aAdded §aline §ato §afile §asuccessfully");
                                }
                                catch (Exception ex)
                                {
                                    SendCommand("say §cFailed §cto §cadd §cline §cto §cfile: " + ex.Message);
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
                                    if (!int.TryParse(chatMessage.Split(new string[] { "@!viewtext" }, StringSplitOptions.None)[1].Trim(), out page))
                                    {
                                        page = 1;
                                    }
                                    string[] fileLines = File.ReadAllLines(Path.GetDirectoryName(ServerManager.ServerJarPath) + ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\')[ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\').Length - 1] + ".txt");
                                    SendCommand("tellraw " + user + " [\"\",{\"text\":\"Page " + page + "/" + Math.Ceiling(fileLines.Length / 5f) + "\",\"color\":\"aqua\"}]");
                                    for (int i = 5 * page - 5; i < 5 * page; i++)
                                    {
                                        SendCommand("tellraw " + user + " [\"\",{\"text\":\"" + i + 1 + ": " + fileLines[i] + "\",\"color\":\"aqua\"}]");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SendCommand("say §cFailed §cto §cread §cfile: " + ex.Message);
                                }
                            }
                            //Deletes the specefied line from the text file
                            //Example:
                            //@!delline 2
                            else if (chatMessage.TrimStart().StartsWith("@!delline "))
                            {
                                try
                                {
                                    int line = Convert.ToInt32(chatMessage.Split(new string[] { "@!delline" }, StringSplitOptions.None)[1].Trim()) - 1;
                                    string path = Path.GetDirectoryName(ServerManager.ServerJarPath) + ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\')[ServerManager.MinecraftServer.StartInfo.WorkingDirectory.Split('\\').Length - 1] + ".txt";
                                    List<string> fileLines = File.ReadAllLines(path).ToList();
                                    fileLines.RemoveAt(line);
                                    File.WriteAllLines(path, fileLines);
                                }
                                catch (Exception ex)
                                {
                                    SendCommand("say §cFailed §cto §cdelete §cline: " + ex.Message);
                                }
                            }
                            //Shows all commands, their syntax, and their description
                            //TODO: Allow user-made commands to have a help message
                            //Example:
                            //@!help
                            //TODO: @!help import
                            else if (chatMessage.TrimStart().StartsWith("@!help"))
                            {
                                SendCommand("tellraw " + user + " [\"\",{\"text\":\"@!import [link|filepath]: Imports a remote MCCBL File\",\"color\":\"yellow\"}]");
                                SendCommand("tellraw " + user + " [\"\",{\"text\":\"@!reimport [selector]: Reimports last locally imported file (no links)\",\"color\":\"yellow\"}]");
                                SendCommand("tellraw " + user + " [\"\",{\"text\":\"@!restart: Restarts the server\",\"color\":\"yellow\"}]");
                                SendCommand("tellraw " + user + " [\"\",{\"text\":\"@!addtext [text]: Adds a line of text to the server text file\",\"color\":\"yellow\"}]");
                                SendCommand("tellraw " + user + " [\"\",{\"text\":\"@!viewtext [page]: Shows a page from the server text file\",\"color\":\"yellow\"}]");
                                SendCommand("tellraw " + user + " [\"\",{\"text\":\"@!delline [line number]: Deletes a line of text from the server text file\",\"color\":\"yellow\"}]");
                                foreach (string file in Directory.GetFileSystemEntries(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + "\\commands"))
                                {
                                    SendCommand("tellraw " + user + " [\"\",{\"text\":\"" + Path.GetFileName(file).Replace(".mccbl", "") + "\",\"color\":\"yellow\"}]");
                                }
                            }
                            //If no command was found, tell user it doesn't exist
                            else if (!wasUserMade)
                            {
                                SendCommand("tellraw " + user + " [\"\",{\"text\":\"[Error] Unknown Command, Type @!help for help\",\"color\":\"red\"}]");
                            }

                            //Add chat messages to server console window
                            ConsoleWindow.SelectionColor = Color.Violet;
                            string timestringa = DateTime.Now.ToString("M/d HH:mm:ss");
                            ConsoleWindow.AppendText(string.Format("\r\n[{0}]: <{2}> {1}", timestringa, chatMessage.Trim(), ChatTools.FilterUsername(ChatTools.FilterCommand(ServerManager.LastRecievedMessage))));
                            ConsoleWindow.ScrollToCaret();
                            ConsoleWindow.SelectionColor = Color.White;
                            return;
                        }
                        //Add other messages to server console window
                        string timestring = DateTime.Now.ToString("M/d HH:mm:ss");
                        ConsoleWindow.AppendText(string.Format("\r\n[{0}]: {1}", timestring, ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Trim()));
                        ConsoleWindow.ScrollToCaret();
                        ConsoleWindow.SelectionColor = Color.White;
                    }
                });
            }
            catch
            {

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
                Invoke((MethodInvoker)delegate
                {
                    if (ServerManager.LastRecievedMessage != null && ServerManager.LastRecievedMessage.Trim() != "")
                    {
                        ConsoleWindow.SelectionColor = Color.Red;
                        string timestring = DateTime.Now.ToString("M/d HH:mm:ss");
                        ConsoleWindow.AppendText(string.Format("\r\n[{0}]: {1}", timestring, ChatTools.FilterCommand(ServerManager.LastRecievedMessage).Trim()));
                        ConsoleWindow.ScrollToCaret();
                        ConsoleWindow.SelectionColor = Color.White;
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
        private void SendCommandButton_Click(object sender, EventArgs e)
        {
            if (CommandInput.Text.Trim() != "" && ServerManager.MinecraftServer != null)
            {
                ServerManager.HistoryIndex = -1;
                ServerManager.MinecraftServer.StandardInput.WriteLine(CommandInput.Text);
                ServerManager.MinecraftServer.StandardInput.FlushAsync();
                ConsoleWindow.SelectionColor = Color.Cyan;
                ServerManager.SentCommandsList.Add(CommandInput.Text);
                string timestring = DateTime.Now.ToString("M/d HH:mm:ss");
                ConsoleWindow.AppendText(string.Format("\r\n[{0}]: {1}", timestring, CommandInput.Text));
                ConsoleWindow.ScrollToCaret();
                ConsoleWindow.SelectionColor = Color.White;
                CommandInput.Text = "";
            }
        }

        /// <summary>
        /// Activates when the window is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
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
            if (e.KeyCode == Keys.Up)
            {
                if (ServerManager.HistoryIndex < ServerManager.SentCommandsList.Count - 1)
                {
                    ServerManager.HistoryIndex++;
                    CommandInput.Text = ServerManager.SentCommandsList[ServerManager.SentCommandsList.Count - ServerManager.HistoryIndex - 1];
                    CommandInput.SelectionStart = CommandInput.Text.Length;
                    CommandInput.SelectionLength = 0;
                }
            }
            else if (e.KeyCode == Keys.Down)
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
                if (ChooseFileDialog.ShowDialog() != DialogResult.Cancel)
                {
                    CBLInterpreter interpreter = new CBLInterpreter(this, ChooseFileDialog.SafeFileName);
                    ConsoleWindow.AppendText("Importing " + ChooseFileDialog.SafeFileName);
                    SendCommand("say §6Importing §b§l" + ChooseFileDialog.SafeFileName);
                    interpreter.Interpret(ChooseFileDialog.FileName);
                    ConsoleWindow.AppendText("Successfully imported " + interpreter.commands.Count + " commands");
                    SendCommand("say §aSuccessfully §aimported §e§l" + interpreter.commands.Count + " §acommands");
                    SendCommand("say §c§l§nDon't §c§l§nforget §cto §cenable §cthe §cfirst §cCommand §cBlock §cif §cnecessary");
                }
                else
                {
                    SendCommand("say §cImport §cCancelled/Failed §b§l");
                }
            }
            else
            {
                //Reimports the previously imported commands
                if (ChooseFileDialog.SafeFileName.EndsWith(".mccbl"))
                {
                    CBLInterpreter interpreter = new CBLInterpreter(this, ChooseFileDialog.SafeFileName, false, selector ?? Properties.Settings.Default.LastSelector);
                    ConsoleWindow.AppendText("Importing " + ChooseFileDialog.SafeFileName);
                    SendCommand("say §6Reimporting §b§l" + ChooseFileDialog.SafeFileName);
                    interpreter.Interpret(ChooseFileDialog.FileName);
                    ConsoleWindow.AppendText("Successfully imported " + interpreter.commands.Count + " commands");
                    SendCommand("say §aSuccessfully §areimported §e§l" + interpreter.commands.Count + " §acommands");
                    SendCommand("say §c§l§nDon't §c§l§nforget §cto §cenable §cthe §cfirst §cCommand §cBlock §cif §cnecessary");
                }
                else
                {
                    SendCommand("say §cReimporting §b§l" + ChooseFileDialog.SafeFileName + " §cFailed!");
                }
            }
        }

        /// <summary>
        /// Activates when you click the stop server button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stopToolStripMenuItem.IsEnabled() && MessageBox.Show("Are you sure you want to stop the server?", "Stop Server?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ServerManager.Restart = false;
                ServerManager.Switch = false;
                Close();
                startToolStripMenuItem.Enable();
                stopToolStripMenuItem.Disable();
                restartToolStripMenuItem.Disable();
                switchToolStripMenuItem.Disable();
                toolsToolStripMenuItem.Enabled = false;
            }
        }

        /// <summary>
        /// Activates when you click the start server button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (startToolStripMenuItem.IsEnabled())
            {
                StartServer();
            }
        }

        /// <summary>
        /// Opens the OPs file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void operatorsMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + @"\ops.json");
        }

        /// <summary>
        /// Opens the server.properties file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propertiesMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + @"\server.properties");
        }

        /// <summary>
        /// Opens the whitelist.json file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void whitelistMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + @"\whitelist.json");
        }

        /// <summary>
        /// Saves the set min RAM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minRAM_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.MinRAM = minRAM.Text;
            Settings.Default.Save();
        }

        /// <summary>
        /// Saves the set max RAM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maxRAM_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.MaxRAM = maxRAM.Text;
            Settings.Default.Save();
        }

        /// <summary>
        /// Activates on startup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            ConsoleWindow.BackColor = Color.FromArgb(47, 47, 47);
            CommandInput.BackColor = Color.FromArgb(47, 47, 47);
            minRAM.Text = Settings.Default.MinRAM;
            maxRAM.Text = Settings.Default.MaxRAM;
            MainWindowToolStrip.Renderer = new ToolStripProfessionalRenderer(new ToolStripRenderTable());
        }

        /// <summary>
        /// Shows the about dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutDialog()).ShowDialog();
        }

        /// <summary>
        /// Activates when clicking the import commands menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importCommandBlockLanguageFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (importCommandBlockLanguageFileToolStripMenuItem.IsEnabled())
            {
                ImportFile();
            }
        }

        /// <summary>
        /// Activates when clicking the restart menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (restartToolStripMenuItem.IsEnabled() && MessageBox.Show("Are you sure you want to ServerManager.Restart?", "Confirm ServerManager.Restart", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
        private void switchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (switchToolStripMenuItem.IsEnabled() && MessageBox.Show("Are you sure you want to switch servers?", "Confirm Switch Server", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            (new HelpWindow()).Show();
        }

        /// <summary>
        /// Activates when clicking the server option menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serverOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //(new ServerSettingsWindow()).Show();
            MessageBox.Show("Not Yet Implemented");
        }
    }
}
