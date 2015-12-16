using System.Drawing;
using System.Windows.Forms;

namespace ExtMethods
{
    public static class Ext
    {
        public static bool ContainsAny(this string input, string[] checkContains)
        {
            foreach (string s in checkContains)
            {
                if (input.Contains(s))
                {
                    return true;
                }
            }

            return false;
        }

        public static string ReplaceAny(this string input, string[] checkReplace, string replaceString)
        {
            foreach (string s in checkReplace)
            {
                input.Replace(s, replaceString);
            }

            return input;
        }

        public static void Disable(this ToolStripMenuItem controlToDisable)
        {
            controlToDisable.ForeColor = Color.LightCoral;
        }

        public static void Enable(this ToolStripMenuItem controlToEnable)
        {
            controlToEnable.ForeColor = Color.White;
        }

        public static bool IsEnabled(this ToolStripMenuItem controlToCheck)
        {
            return (controlToCheck.ForeColor == Color.White);
        }
    }
}
