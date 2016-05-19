using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBlockLanguageInterpreter
{
    public class CBLFile
    {
        public List<Command> Commands;
        public Dictionary<string, string> Properties;
        public MainWindow Window;
        /// <summary>
        /// Alias to Properties
        /// </summary>
        public Dictionary<string, string> AtTags
        {
            get { return Properties; }
            set { Properties = value; }
        }
        public string FileName;
        public string[] FileLines;
        public string Selector;
        private bool webLink;

        public CBLFile(List<Command> commands, Dictionary<string, string> atTags, string fileName, string[] fileLines, string selector, bool link)
        {
            Commands = commands;
            Properties = atTags;
            FileName = fileName;
            FileLines = fileLines;
            Selector = selector;
            webLink = link;
        }

        /// <summary>
        /// Executes the current CBLFile directly into the current ServerManager
        /// </summary>
        /// <param name="user"></param>
        /// <param name="suffix"></param>
        public void Execute(string user, string suffix)
        {
            string[] args = suffix.Split(' ');
            foreach (Command c in Commands)
            {
                string text = c.CommandText;
                text = text.Replace("{arg}", suffix).Replace("{user}", user);

                for (int i = 1; i < args.Length + 1; i++)
                {
                    text = text.Replace("{arg" + i + "}", args[i - 1]);
                }

                ServerManager.MinecraftServer.StandardInput.WriteLine(text);
                ServerManager.MinecraftServer.StandardInput.Flush();
            }
        }

        /*
        private int GetFacingDirection(int xmax, int zmax, int x, int y, int z, int index)
        {
            //5 forward
            //3 right
            //4 backwards
            //1 up
            //2 left

            //WAIT THIS WON'T WORK IF THERE ARE AN EVEN NUMBER OF ROWS/COLUMNS
            if (y % 2 == 0)
            {
                if (z % 2 == 0 && x > 0 && x < xmax) { return 5; }
                else if ((z % 2 == 0 && x == xmax) || (z % 2 == 1 && x == 1)) { return 3; }
                else if (z == zmax - 1 && x == xmax) { return 1; }
            }
            else
            {
                if (z % 2 == 0 && x > 0 && x < xmax) { return 5; }
                else if ((z % 2 == 0 && x == xmax) || (z % 2 == 1 && x == 1)) { return 3; }
                else if (z == zmax - 1 && x == xmax) { return 1; }
            }

        }
        */

        /// <summary>
        /// Imports the current CBLFile as command blocks using the current ServerManager
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public bool Import(MainWindow window)
        {
            Window = window;

            //Don't know what this bit here does, but I don't want to remove it
            string fileInformation = "";
            foreach (string l in FileLines)
            {
                fileInformation += "\n" + l;
            }
            //*****

            if (Selector != null || webLink || new ScriptLoader(FileName, FileLines, Commands, Properties).ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                bool repeat = Properties.ContainsKey("TYPE") && Properties["TYPE"] == "REPEAT";
                bool powered = !(!Properties.ContainsKey("CHAIN") || Properties["CHAIN"] == "OFF");

                int loopedCommands = 0;
                int yOffset = 0;

                #region WIP New Code
                /*
                TEMP
                int xmax = 5;
                int zmax = 5;

                while (loopedCommands < Commands.Count)
                {
                    for (int z = 0; z < zmax; z++)
                    {
                        for (int x = 1; x < xmax + 1; x++)
                        {
                            if (Commands.Count == loopedCommands) { break; };
                            string facing = GetFacingDirection(xmax, zmax, x, yOffset, z, loopedCommands).ToString();

                            string commandToSend = BuildCommand(Commands[loopedCommands], x, yOffset, z, facing, repeat, powered);
                            ServerManager.MinecraftServer.StandardInput.WriteLine(commandToSend);
                            ServerManager.MinecraftServer.StandardInput.Flush();
                            loopedCommands++;
                        }
                    }
                    yOffset++;
                    for (int z = 0; z < zmax; z++)
                    {
                        for (int x = 1; x < xmax + 1; x++)
                        {
                            if (Commands.Count == loopedCommands) { break; };
                            string facing = GetFacingDirection(xmax, zmax, xmax - x, yOffset, zmax - z, loopedCommands).ToString();

                            string commandToSend = BuildCommand(Commands[loopedCommands], xmax - x, yOffset, zmax - z, facing, repeat, powered);
                            ServerManager.MinecraftServer.StandardInput.WriteLine(commandToSend);
                            ServerManager.MinecraftServer.StandardInput.Flush();
                            loopedCommands++;
                        }
                    }
                    yOffset++;
                }

                */
                #endregion

                while (loopedCommands < Commands.Count)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        if (Commands.Count != loopedCommands)
                        {
                            //TODO: Find some way to fix this conditional problem
                            //May not be fixable in the current implementation
                            //See https://github.com/aopell/MCCBL/blob/master/Examples/Conditionals%20Issue.png
                            if (Commands[loopedCommands].CommandType == Command.Type.Conditional && new int[] { 0, 4, 5, 9, 10, 14, 15, 19, 20, 24 }.Contains(i))
                            {
                                ServerManager.MinecraftServer.StandardInput.WriteLine(ChatTools.Tellraw("@a", TellrawColor.red, "[WARNING] Conditional command not supported at height " + yOffset + " command index " + i));
                                ServerManager.MinecraftServer.StandardInput.WriteLine(ChatTools.Tellraw("@a", TellrawColor.red, "[WARNING] Problem command: " + Commands[loopedCommands].CommandText));
                                ServerManager.MinecraftServer.StandardInput.Flush();
                            }

                            //UGLY IF STATEMENTS
                            if (yOffset % 2 == 0)
                            {
                                string commandToSend = "";
                                if (i >= 0 && i < 4) { commandToSend = BuildCommand(Commands[loopedCommands], i + 1, yOffset, 0, "5", repeat, powered); }
                                else if (i == 4) { commandToSend = BuildCommand(Commands[loopedCommands], 5, yOffset, 0, "3", repeat, powered); }
                                else if (i > 4 && i < 9) { commandToSend = BuildCommand(Commands[loopedCommands], 10 - i, yOffset, 1, "4", repeat, powered); }
                                else if (i == 9) { commandToSend = BuildCommand(Commands[loopedCommands], 1, yOffset, 1, "3", repeat, powered); }
                                else if (i > 9 && i < 14) { commandToSend = BuildCommand(Commands[loopedCommands], i - 9, yOffset, 2, "5", repeat, powered); }
                                else if (i == 14) { commandToSend = BuildCommand(Commands[loopedCommands], 5, yOffset, 2, "3", repeat, powered); }
                                else if (i > 14 && i < 19) { commandToSend = BuildCommand(Commands[loopedCommands], 20 - i, yOffset, 3, "4", repeat, powered); }
                                else if (i == 19) { commandToSend = BuildCommand(Commands[loopedCommands], 1, yOffset, 3, "3", repeat, powered); }
                                else if (i > 19 && i < 24) { commandToSend = BuildCommand(Commands[loopedCommands], i - 19, yOffset, 4, "5", repeat, powered); }
                                else if (i == 24) { commandToSend = BuildCommand(Commands[loopedCommands], 5, yOffset, 4, "1", repeat, powered); }
                                ServerManager.MinecraftServer.StandardInput.WriteLine(commandToSend);
                                ServerManager.MinecraftServer.StandardInput.Flush();
                            }
                            //0: down -y 1: up +y 2: north -z 3: south +z 4: west -x 5: east +x
                            else
                            {
                                string commandToSend = "";
                                if (i >= 0 && i < 4) { commandToSend = BuildCommand(Commands[loopedCommands], 5 - i, yOffset, 4, "4", repeat, powered); }
                                else if (i == 4) { commandToSend = BuildCommand(Commands[loopedCommands], 1, yOffset, 4, "2", repeat, powered); }
                                else if (i > 4 && i < 9) { commandToSend = BuildCommand(Commands[loopedCommands], i - 4, yOffset, 3, "5", repeat, powered); }
                                else if (i == 9) { commandToSend = BuildCommand(Commands[loopedCommands], 5, yOffset, 3, "2", repeat, powered); }
                                else if (i > 9 && i < 14) { commandToSend = BuildCommand(Commands[loopedCommands], 15 - i, yOffset, 2, "4", repeat, powered); }
                                else if (i == 14) { commandToSend = BuildCommand(Commands[loopedCommands], 1, yOffset, 2, "2", repeat, powered); }
                                else if (i > 14 && i < 19) { commandToSend = BuildCommand(Commands[loopedCommands], i - 14, yOffset, 1, "5", repeat, powered); }
                                else if (i == 19) { commandToSend = BuildCommand(Commands[loopedCommands], 5, yOffset, 1, "2", repeat, powered); }
                                else if (i > 19 && i < 24) { commandToSend = BuildCommand(Commands[loopedCommands], 25 - i, yOffset, 0, "4", repeat, powered); }
                                else if (i == 24) { commandToSend = BuildCommand(Commands[loopedCommands], 1, yOffset, 0, "1", repeat, powered); }
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
                ServerManager.MinecraftServer.StandardInput.WriteLine("execute " + Selector + " ~ ~ ~ blockdata ~ ~ ~ {Text1:\"[{\\\"text\\\":\\\"" + (!webLink ? FileName.Replace(".mccbl", "") : "Remote Import") + "\\\"}]\"" + ",Text2:\"[{\\\"text\\\":\\\"" + Commands.Count + " Commands" + "\\\"}]\"" + ",Text3:\"[{\\\"text\\\":\\\"" + DateTime.Now.ToString("dd MMM yyyy") + "\\\"}]\"" + ",Text4:\"[{\\\"text\\\":\\\"" + DateTime.Now.ToString("HH:mm:ss") + "\\\"}]\"}");
                ServerManager.MinecraftServer.StandardInput.Flush();
            }
            else
            {
                return false;
            }

            return true;
        }

        private string BuildCommand(Command command, int x, int y, int z, string facing, bool repeat, bool powered)
        {
            string commandBase = string.Format("execute {0} ~ ~ ~ setblock ", Selector);
            if (command.CommandType == Command.Type.RepeatingConditional)
            {
                commandBase += string.Format("~{0} ~{1} ~{2} {4} {3} replace ", x, y, z, (Convert.ToInt32(facing) + 8).ToString(), "minecraft:repeating_command_block");
            }
            else
            {
                commandBase += string.Format("~{0} ~{1} ~{2} {4} {3} replace ", x, y, z, command.CommandType == Command.Type.Conditional ? (Convert.ToInt32(facing) + 8).ToString() : facing, command.CommandIndex == 0 ? (repeat ? "minecraft:repeating_command_block" : "minecraft:command_block") : "minecraft:chain_command_block");
            }
            commandBase += "{Command:" + command.CommandText + (command.CommandIndex != 0 && powered ? ",auto:1b" : "") + "}";
            MainWindow.TextToAdd = commandBase;
            Window.UpdateText();
            return commandBase;
        }
    }
}
