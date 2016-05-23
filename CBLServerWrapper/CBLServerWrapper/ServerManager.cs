using CBLServerWrapper.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CBLServerWrapper
{
    public static class ServerManager
    {
        public static string DefaultWindowTitle = "Minecraft Server | Server Offline";

        public static Process MinecraftServer = null;
        public static string ServerJarPath;
        public static string LastRecievedMessage;
        public static int HistoryIndex;
        public static List<string> SentCommandsList = new List<string>();
        public static List<string> ChatHistory = new List<string>();
        public static MainWindow ConsoleWindow = null;
        public static bool Restart = false;
        public static bool Switch = false;
        public static List<string> LoggedInPlayers = new List<string>();
        public static string WorldName;
        /// <summary>
        /// Called to start a server
        /// </summary>
        /// <param name="fileName">location of server jar</param>
        /// <param name="mainWindow">window target</param>
        public static void StartServer(string fileName, MainWindow mainWindow)
        {
            MinecraftServer = new Process();
            MinecraftServer.StartInfo.WorkingDirectory = Path.GetDirectoryName(Restart ? ServerJarPath : fileName);
            MinecraftServer.StartInfo.Arguments = string.Format("-Xmx{0} -Xms{1} -jar \"" + (Restart ? ServerJarPath : fileName) + "\" nogui", Settings.Default.MaxRAM, Settings.Default.MinRAM);
            MinecraftServer.StartInfo.FileName = "java";
            MinecraftServer.StartInfo.UseShellExecute = false;
            MinecraftServer.StartInfo.RedirectStandardOutput = true;
            MinecraftServer.StartInfo.RedirectStandardError = true;
            MinecraftServer.StartInfo.RedirectStandardInput = true;
            MinecraftServer.StartInfo.CreateNoWindow = true;
            MinecraftServer.ErrorDataReceived += MinecraftServer_ErrorDataReceived;
            MinecraftServer.OutputDataReceived += MinecraftServer_OutputDataReceived;
            MinecraftServer.EnableRaisingEvents = true;
            MinecraftServer.Start();
            MinecraftServer.BeginOutputReadLine();
            MinecraftServer.BeginErrorReadLine();
            MinecraftServer.Exited += MinecraftServer_Exited;
            ServerJarPath = fileName;
            ConsoleWindow = mainWindow;
            mainWindow.Title = "Minecraft Server | " + GetWorldName();
        }

        private static void MinecraftServer_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ConsoleWindow.ErrorDataRecieved(e.Data);
        }

        private static void MinecraftServer_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ConsoleWindow.OutputDataRecieved(e.Data);
        }

        private static void MinecraftServer_Exited(object sender, EventArgs e)
        {
            ConsoleWindow.ServerExited();
        }
        /// <summary>
        /// Finds the name of the current world
        /// </summary>
        /// <param name="serverDir">Folder where the server is located</param>
        public static string GetWorldName()
        {
            string[] lines = File.ReadAllLines(MinecraftServer.StartInfo.WorkingDirectory + @"\server.properties");
            foreach (string line in lines)
            {
                if (line.Contains("level-name="))
                {
                    return line.Replace("level-name=", "").Trim();
                }
            }
            return "";
        }
    }
}
