﻿namespace CBLServerWrapper
{
    public class Command
    {
        public string CommandText;
        public Type CommandType;
        public int CommandIndex;

        public Command(string command, Type commandType, int index)
        {
            CommandText = command;
            CommandType = commandType;
            CommandIndex = index;
        }

        public enum Type
        {
            Normal,
            Conditional,
            RepeatingConditional
        }
    }
}
