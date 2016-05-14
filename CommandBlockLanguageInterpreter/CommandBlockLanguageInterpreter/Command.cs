using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandBlockLanguageInterpreter
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
