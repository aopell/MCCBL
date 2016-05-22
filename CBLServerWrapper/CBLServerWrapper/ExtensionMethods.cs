using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CBLServerWrapper
{
    public static class ExtensionMethods
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

        public static void AppendColoredText(this RichTextBox box, string text, SolidColorBrush color)
        {
            TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
            tr.Text = text;
            try
            {
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            }
            catch (FormatException) { }
        }

        public static string ReplaceAny(this string input, string[] checkReplace, string replaceString)
        {
            foreach (string s in checkReplace)
            {
                input.Replace(s, replaceString);
            }

            return input;
        }

        public static string CommandArgument(this string message, string commandText)
        {
            return message.Split(new string[] { commandText }, StringSplitOptions.None)[1].Trim();
        }
    }
}
