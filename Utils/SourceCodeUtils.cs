using System;
using System.CodeDom;
using System.IO;
using System.Text;

namespace Erasystemlevel.Utils
{
    public class SourceCodeUtils
    {
        public static string getProgramText(string filePath)
        {
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return getProgramText(stream);
        }

        public static string getProgramText(FileStream fs)
        {
            fs.Position = 0;
            var reader = new StreamReader(fs, Encoding.UTF8);

            return reader.ReadToEnd();
        }

        public static string getHighlightedCode(string text, int? line, int? symbol, int? errorSize)
        {
            if (line == null || symbol == null || text == null)
            {
                return null;
            }

            var res = getLine(text, (int) line) + "\n" + getHighlight((int) symbol, errorSize);

            return res;
        }

        private static string getLine(string text, int line)
        {
            var lines = text.Split('\n');

            return lines.Length < line ? "" : lines[line - 1];
        }

        private static string getHighlight(int offset, int? length)
        {
            if (length == null || length <= 0)
            {
                length = 1;
            }

            var dashesLen = Math.Max(0, offset - 1);
            var dashes = new string('-', dashesLen);

            var pointer = new string('^', (int) length);

            return dashes + pointer;
        }
    }
}