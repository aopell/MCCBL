using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CBLServerWrapper
{
    public static class ChatTools
    {
        /// <summary>
        /// Gets the username from a chat message
        /// </summary>
        /// <param name="userInput">The chat message to filter</param>
        /// <returns>Username of a filtered command</returns>
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
        /// <returns>A filtered command message</returns>
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

        /// <summary>
        /// Generates a tellraw command with the given options
        /// </summary>
        /// <param name="selector">Who will receive the message</param>
        /// <param name="color">The color of the message</param>
        /// <param name="message">The message to send</param>
        /// <returns>A correctly formatted tellraw command</returns>
        public static string Tellraw(string selector, TellrawColor color, string message)
        {
            return "tellraw " + selector + " [\"\",{\"text\":\"" + message + "\",\"color\":\"" + color.ToString() + "\"}]";
        }

        /// <summary>
        /// Generates a tellraw command with the given options
        /// Concatenates all of the messages into one message
        /// </summary>
        /// <param name="selector">Who will receive the message</param>
        /// <param name="colors">Colors for the corresponding messages</param>
        /// <param name="messages">Messages to send</param>
        /// <returns></returns>
        public static string MultiTellraw(string selector, TellrawColor[] colors, string[] messages)
        {
            string command = $"tellraw {selector} [\"\"";
            for (int i = 0; i < messages.Length; i++)
            {
                command += ",{\"text\":\"" + messages[i] + "\",\"color\":\"" + colors[i].ToString() + "\"}";
            }
            return command + "]";
        }
    }

    public enum TellrawColor
    {
        black,
        dark_blue,
        dark_green,
        dark_aqua,
        dark_red,
        dark_purple,
        gold,
        gray,
        dark_gray,
        blue,
        green,
        aqua,
        red,
        light_purple,
        yellow,
        white
    }
}
