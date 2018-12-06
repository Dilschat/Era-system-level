using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Erasystemlevel.Tokenizer
{
    public class Tokenizer
    {
        readonly Regex numericRegex = new Regex("^(\\+|-)?\\d+$");
        readonly Regex identifierRegex = new Regex("\\b([A-Za-z][A-Za-z0-9_]*)\\b");
        private readonly Regex register = new Regex("\\bR([0-9]|[12][0-9]|3[01])\\b");
        private int curLine;

        readonly HashSet<string> delimeters = new HashSet<string>(new List<string>()
        {
            ";", ",", ".", "(", ")", "[", "]", "//"
        });

        readonly HashSet<string> operators = new HashSet<string>(new List<string>()
        {
            "+", "-", "*", "&", "|", "^", "?", "=", "<", ">", "/=", ":=",
            "+=", "-=", ">>=", "<<=", "|=", "&=", "^=", "<=", ">=", "?=",
            "<=>"
        });

        readonly HashSet<string> keywords = new HashSet<string>(new List<string>()
        {
            "if", "else", "int", "short", "byte", "const", "routine", "do", "end",
            "start", "entry", "skip", "stop", "goto", "format", "for", "from", "to",
            "step", "while", "loop", "break", "then", "by", "trace", "data", "module", "code",
            "this"
        });

        private readonly StreamReader reader;

        public int GetCurLine()
        {
            return curLine;
        }


        public Tokenizer(string filePath)
        {
            var fs = new FileStream(filePath, FileMode.Open,
                FileAccess.Read);
            reader = new StreamReader(fs, Encoding.UTF8);
        }

        public Token Tokenize()
        {
            var currentToken = string.Empty;

            while (!reader.EndOfStream)
            {
                var next = Convert.ToChar(reader.Read());
                if (next.ToString() == "\n")
                {
                    curLine = +1;
                }

                if (next.ToString() == "" || next.ToString() == "\n" || next.ToString() == " ")
                {
                    continue;
                }

                currentToken += next;
                //Console.WriteLine(currentToken);

                if (delimeters.Contains(currentToken))
                {
                    return new Token(Token.TokenType.Delimiter, currentToken);
                }

                if (register.IsMatch(currentToken))
                {
                    var nextChar = Convert.ToChar(reader.Peek());

                    if (!char.IsDigit(nextChar) && !char.IsLetter(nextChar) && nextChar.ToString() != "_")
                    {
                        return new Token(Token.TokenType.Register, currentToken);
                    }
                }

                if (operators.Contains(currentToken))
                {
                    while (operators.Contains(currentToken + Convert.ToChar(reader.Peek())))
                    {
                        currentToken = currentToken + Convert.ToChar(reader.Read());
                    }

                    return new Token(Token.TokenType.Operator, currentToken);
                }

                if (keywords.Contains(currentToken))
                {
                    if (reader.EndOfStream || Convert.ToString(reader.Peek()).Equals(" ") ||
                        Convert.ToString(this.reader.Peek()).Equals("/n"))
                    {
                        return new Token(Token.TokenType.Keyword, currentToken);
                    }

                    var nextChar = Convert.ToChar(reader.Peek());
                    if (!char.IsDigit(nextChar) && !char.IsLetter(nextChar) && nextChar.ToString() != "_")
                    {
                        return new Token(Token.TokenType.Keyword, currentToken);
                    }
                }

                if (identifierRegex.IsMatch(currentToken))
                {
                    if (reader.EndOfStream || Convert.ToString(reader.Peek()).Equals(" ") ||
                        Convert.ToString(reader.Peek()).Equals("/n"))
                    {
                        return new Token(Token.TokenType.Identifier, currentToken);
                    }

                    var nextChar = Convert.ToChar(reader.Peek());
                    if ((char.IsDigit(nextChar) || char.IsLetter(nextChar)) && nextChar != ' ')
                    {
                        continue;
                    }

                    return new Token(Token.TokenType.Identifier, currentToken);
                }

                if (!numericRegex.IsMatch(currentToken)) continue;
                if (reader.EndOfStream)
                {
                    return new Token(Token.TokenType.Number, currentToken);
                }

                while (numericRegex.IsMatch(currentToken + Convert.ToChar(reader.Peek())) &&
                       !reader.EndOfStream)
                {
                    currentToken = currentToken + Convert.ToChar(reader.Read());
                }

                return new Token(Token.TokenType.Number, currentToken);
            }

            reader.Close();
            return null;
        }
    }
}