using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CBLServerWrapper
{
    public class CBLInterpreter
    {
        public string TextToAdd = "";
        public string FileName;
        public static string Selector;
        bool webLink;
        public List<Command> commands = new List<Command>();

        public CBLInterpreter(MainWindow callingClass, string fileName, bool link = false, string selector = null)
        {
            FileName = fileName;
            webLink = link;
            Selector = selector;
        }

        public CBLFile Interpret(string filePath)
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
                        return null;
                    }
                }
            }

            MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControl();
            sc.Language = "VBScript";

            Dictionary<string, string> constants = new Dictionary<string, string>();
            Dictionary<string, string> atTags = new Dictionary<string, string>();

            foreach (string line in fileLines)
            {
                if (line.Trim().StartsWith("@") && line.Trim()[1] != '!')
                {
                    string key = line.Trim().Split(' ')[0].Substring(1);
                    string val = string.Join(" ", line.Trim().Split(' ').Skip(1));

                    atTags.Add(key, val);
                }
                else if (line.Trim().StartsWith("#define"))
                {
                    string key = line.Trim().Split(' ')[1];
                    string val = string.Join(" ", line.Trim().Split(' ').Skip(2));

                    if (constants.ContainsKey(key))
                    {
                        constants.Remove(key);
                        constants.Add(key, val);
                    }
                    else
                    {
                        constants.Add(key, val);
                    }
                }
                else if (!line.Trim().StartsWith("//") && !line.Trim().StartsWith("@") && !line.Trim().StartsWith("::") && !line.Trim().StartsWith(";;") && line.Trim() != "")
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
                            ServerManager.MinecraftServer.StandardInput.WriteLine(ChatTools.Tellraw("@a", TellrawColor.red, $"[ERROR] Failed to interpret math statement ({expression}): {ex.ToString()}"));
                            ServerManager.MinecraftServer.StandardInput.FlushAsync();
                        }
                    }
                    else
                    {
                        output = input;
                    }

                    foreach (string constant in constants.Keys)
                    {
                        if (output.Contains("#" + constant + "#"))
                        {
                            output.Replace("#" + constant + "#", constants[constant]);
                        }
                    }

                    if (line.Trim().StartsWith("??"))
                    {
                        commands.Add(new Command(output.Trim().Substring(2).Replace("@!", ""), Command.Type.RepeatingConditional, commands.Count));
                    }
                    else if (line.Trim().StartsWith("?"))
                    {
                        commands.Add(new Command(output.Trim().Substring(1).Replace("@!", ""), Command.Type.Conditional, commands.Count));
                    }
                    else
                    {
                        commands.Add(new Command(output.Replace("@!", ""), Command.Type.Normal, commands.Count));
                    }
                }
            }


            return new CBLFile(commands, atTags, FileName, fileLines, Selector, webLink);
        }
    }
}
