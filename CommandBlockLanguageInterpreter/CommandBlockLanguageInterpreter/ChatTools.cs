using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CommandBlockLanguageInterpreter
{
    public static class ChatTools
    {
        /// <summary>
        /// Gets the username from a chat message
        /// </summary>
        /// <param name="userInput">The chat message to filter</param>
        /// <returns></returns>
        public static string FilterUsername(string userInput)
        {
            userInput = userInput.Trim().Substring(1).Split('>')[0];
            List<int> indexesToRemove = new List<int>();
            //Removes section symbols and anything directly after them
            for (int i = 0; i < userInput.Length; i++)
            {
                if (userInput[i] == '\u00A7')
                {
                    indexesToRemove.Add(i);
                }
            }

            int corrections = 0;

            foreach (int i in indexesToRemove)
            {
                userInput = userInput.Remove(i - corrections, 2);
                corrections += 2;
            }
            return userInput;
        }

        /// <summary>
        /// Removes unnecessary information from a recieved message from the server
        /// </summary>
        /// <param name="command">The message to filter</param>
        /// <returns></returns>
        public static string FilterCommand(string command)
        {
            string[] splitString = new string[] { " [Server thread/INFO]: ", " [Server thread/WARN]: ", " [Server Shutdown Thread/INFO]: ", " [Server Watchdog/FATAL]: ", " [Server Watchdog/ERROR]: " };
            try
            {
                return command.Split(splitString, StringSplitOptions.None)[1];
            }
            catch (IndexOutOfRangeException)
            {
                try
                {
                    return Regex.Split(command, @" \[User Authenticator #\d+(?!\.)\/INFO]: ")[1];
                }
                catch (IndexOutOfRangeException)
                {
                    try
                    {
                        return Regex.Split(command, @" \[Query Listener #\d+(?!\.)\/INFO]: ")[1];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return command;
                    }
                }
            }
            catch (NullReferenceException)
            {
                return command;
            }
        }
    }
}
