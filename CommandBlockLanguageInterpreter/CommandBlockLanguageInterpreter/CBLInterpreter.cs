using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommandBlockLanguageInterpreter
{
    public class CBLInterpreter
    {
        public string TextToAdd = "";
        public MainWindow CallingClass;
        public string FileName;
        public static string Selector;
        bool webLink;
        public List<Command> commands = new List<Command>();

        public CBLInterpreter(MainWindow callingClass, string fileName, bool link = false, string selector = null)
        {
            CallingClass = callingClass;
            FileName = fileName;
            webLink = link;
            Selector = selector;
        }

        public void Interpret(string filePath)
        {
            int totalCommands = 0;
            string[] fileLines = new string[] { };
            if (!webLink)
            {
                fileLines = System.IO.File.ReadAllLines(filePath);
            }
            else
            {
                if (filePath.StartsWith("http"))
                {
                    new System.Net.WebClient().DownloadFile(filePath, ServerManager.MinecraftServer.StartInfo.WorkingDirectory + "/Remote Import.mccbl");
                    fileLines = System.IO.File.ReadAllLines(ServerManager.MinecraftServer.StartInfo.WorkingDirectory + "/Remote Import.mccbl");
                }
                else
                {
                    try
                    {
                        fileLines = System.IO.File.ReadAllLines(filePath);
                    }
                    catch
                    {
                        return;
                    }
                }
            }
            bool powered = false;
            bool repeat = false;
            int yOffset = 0;
            MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControl();
            sc.Language = "VBScript";

            foreach (string line in fileLines)
            {
                if (!line.Trim().StartsWith("//") && !line.Trim().StartsWith("@") && !line.Trim().StartsWith("::") && !line.Trim().StartsWith(";;") && line.Trim() != "")
                {
                    totalCommands++;

                    string input = line.TrimStart();
                    string output;
                    if (input.Contains("([") && input.Contains("])"))
                    {
                        string expression = input.Split(new string[] { "([", "])" }, StringSplitOptions.None)[1];
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
                            ServerManager.MinecraftServer.StandardInput.WriteLine("tellraw @a [\"\",{\"text\":\"[Error] Failed to interpret math statement: " + ex.Message + "\",\"color\":\"red\"}]");
                            ServerManager.MinecraftServer.StandardInput.FlushAsync();
                        }
                    }
                    else
                    {
                        output = input;
                    }

                    if (line.Trim().StartsWith("?"))
                    {
                        commands.Add(new Command(output.Trim().Substring(1).Replace("@!", ""), Command.Type.Conditional, commands.Count));
                    }
                    else
                    {
                        commands.Add(new Command(output.Replace("@!", ""), Command.Type.Normal, commands.Count));
                    }
                }
            }

            string fileInformation = "";
            foreach (string l in fileLines)
            {
                fileInformation += "\n" + l;
            }

            if (Selector != null || webLink || new ScriptLoader(FileName, fileLines, commands).ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                if (fileLines[0].Contains("REPEAT"))
                {
                    repeat = true;
                }
                else if (fileLines[0].Contains("ONCE"))
                {
                    repeat = false;
                }
                if (fileLines[1].Contains("ON"))
                {
                    powered = true;
                }
                else if (fileLines[1].Contains("OFF"))
                {
                    powered = false;
                }

                int loopedCommands = 0;
                while (loopedCommands < commands.Count)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        if (commands.Count != loopedCommands)
                        {
                            //TODO: Find some way to fix this conditional problem
                            //May not be fixable in the current implementation
                            //See "Transparency2/Utilities/Conditional Issue.png"
                            if (commands[i].CommandType == Command.Type.Conditional && new int[] { 0, 4, 5, 9, 10, 14, 15, 19, 20, 24 }.Contains(i))
                            {
                                ServerManager.MinecraftServer.StandardInput.WriteLine("/tellraw @p [\"\",{\"text\":\"[WARNING] Conditional command not supported at command " + commands[i].CommandText + "\",\"color\":\"red\"}]");
                                ServerManager.MinecraftServer.StandardInput.Flush();
                            }

                            //UGLY IF STATEMENTS
                            if (yOffset % 2 == 0)
                            {
                                string commandToSend = "";
                                if (i >= 0 && i < 4) { commandToSend = BuildCommand(commands[loopedCommands], i + 1, yOffset, 0, "5", repeat, powered); }
                                else if (i == 4) { commandToSend = BuildCommand(commands[loopedCommands], 5, yOffset, 0, "3", repeat, powered); }
                                else if (i > 4 && i < 9) { commandToSend = BuildCommand(commands[loopedCommands], 10 - i, yOffset, 1, "4", repeat, powered); }
                                else if (i == 9) { commandToSend = BuildCommand(commands[loopedCommands], 1, yOffset, 1, "3", repeat, powered); }
                                else if (i > 9 && i < 14) { commandToSend = BuildCommand(commands[loopedCommands], i - 9, yOffset, 2, "5", repeat, powered); }
                                else if (i == 14) { commandToSend = BuildCommand(commands[loopedCommands], 5, yOffset, 2, "3", repeat, powered); }
                                else if (i > 14 && i < 19) { commandToSend = BuildCommand(commands[loopedCommands], 20 - i, yOffset, 3, "4", repeat, powered); }
                                else if (i == 19) { commandToSend = BuildCommand(commands[loopedCommands], 1, yOffset, 3, "3", repeat, powered); }
                                else if (i > 19 && i < 24) { commandToSend = BuildCommand(commands[loopedCommands], i - 19, yOffset, 4, "5", repeat, powered); }
                                else if (i == 24) { commandToSend = BuildCommand(commands[loopedCommands], 5, yOffset, 4, "1", repeat, powered); }
                                ServerManager.MinecraftServer.StandardInput.WriteLine(commandToSend);
                                ServerManager.MinecraftServer.StandardInput.Flush();
                            }
                            //0: down -y 1: up +y 2: north -z 3: south +z 4: west -x 5: east +x
                            else
                            {
                                string commandToSend = "";
                                if (i >= 0 && i < 4) { commandToSend = BuildCommand(commands[loopedCommands], 5 - i, yOffset, 4, "4", repeat, powered); }
                                else if (i == 4) { commandToSend = BuildCommand(commands[loopedCommands], 1, yOffset, 4, "2", repeat, powered); }
                                else if (i > 4 && i < 9) { commandToSend = BuildCommand(commands[loopedCommands], i - 4, yOffset, 3, "5", repeat, powered); }
                                else if (i == 9) { commandToSend = BuildCommand(commands[loopedCommands], 5, yOffset, 3, "2", repeat, powered); }
                                else if (i > 9 && i < 14) { commandToSend = BuildCommand(commands[loopedCommands], 15 - i, yOffset, 2, "4", repeat, powered); }
                                else if (i == 14) { commandToSend = BuildCommand(commands[loopedCommands], 1, yOffset, 2, "2", repeat, powered); }
                                else if (i > 14 && i < 19) { commandToSend = BuildCommand(commands[loopedCommands], i - 14, yOffset, 1, "5", repeat, powered); }
                                else if (i == 19) { commandToSend = BuildCommand(commands[loopedCommands], 5, yOffset, 1, "2", repeat, powered); }
                                else if (i > 19 && i < 24) { commandToSend = BuildCommand(commands[loopedCommands], 25 - i, yOffset, 0, "4", repeat, powered); }
                                else if (i == 24) { commandToSend = BuildCommand(commands[loopedCommands], 1, yOffset, 0, "1", repeat, powered); }
                                ServerManager.MinecraftServer.StandardInput.WriteLine(commandToSend);
                                ServerManager.MinecraftServer.StandardInput.Flush();
                            }
                            loopedCommands++;
                        }
                    }
                    yOffset++;
                }

                ServerManager.MinecraftServer.StandardInput.WriteLine("execute " + Selector + " ~ ~ ~ setblock ~ ~ ~ minecraft:wall_sign 4 replace");
                ServerManager.MinecraftServer.StandardInput.Flush();
                ServerManager.MinecraftServer.StandardInput.WriteLine("execute " + Selector + " ~ ~ ~ blockdata ~ ~ ~ {Text1:\"[{\\\"text\\\":\\\"" + (!webLink ? FileName.Replace(".mccbl", "") : "Remote Import") + "\\\"}]\"" + ",Text2:\"[{\\\"text\\\":\\\"" + commands.Count + " Commands" + "\\\"}]\"" + ",Text3:\"[{\\\"text\\\":\\\"" + DateTime.Now.ToString("dd MMM yyyy") + "\\\"}]\"" + ",Text4:\"[{\\\"text\\\":\\\"" + DateTime.Now.ToString("hh:mm:sstt") + "\\\"}]\"}");
                ServerManager.MinecraftServer.StandardInput.Flush();
            }
        }

        private string BuildCommand(Command command, int x, int y, int z, string facing, bool repeat, bool powered)
        {
            string commandBase = String.Format("execute {0} ~ ~ ~ setblock ", Selector);
            commandBase += String.Format("~{0} ~{1} ~{2} {4} {3} replace ", x, y, z, command.CommandType == Command.Type.Conditional ? (Convert.ToInt32(facing) + 8).ToString() : facing, command.CommandIndex == 0 ? (repeat ? "minecraft:repeating_command_block" : "minecraft:command_block") : "minecraft:chain_command_block");
            commandBase += "{Command:" + command.CommandText + (command.CommandIndex != 0 && powered ? ",auto:1b" : "") + "}";
            MainWindow.TextToAdd = commandBase;
            CallingClass.UpdateText();
            return commandBase;
        }
    }
}
