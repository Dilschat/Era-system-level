using System;
using Erasystemlevel.Utils;

namespace Erasystemlevel.Exception
{
    public class SyntaxError : SystemException
    {
        public const string DefaultMessage = "failed to parse";

        private readonly int? line;
        private readonly int? symbol;
        private readonly int buffSize = 0;

        private string programText;

        public SyntaxError(string message = DefaultMessage) : base(message)
        {
        }

        public SyntaxError(string message, int line, int symbol) : this(message)
        {
            this.line = line;
            this.symbol = symbol;
        }

        public SyntaxError(string message, int line, int symbol, int buffSize) : this(message, line, symbol)
        {
            this.buffSize = buffSize;
        }

        public void setProgramText(string text)
        {
            this.programText = text;
        }

        public string verbose()
        {
            var result = "";
            if (line != null && symbol != null)
            {
                result += "Syntax error in " + line + ":" + symbol + ":" + "\n\n";
            }

            var codePart = SourceCodeUtils.getHighlightedCode(programText, line, symbol - buffSize, buffSize);
            if (codePart != null)
            {
                result += codePart + "\n\n";
            }

            return result + ToString();
        }
    }
}