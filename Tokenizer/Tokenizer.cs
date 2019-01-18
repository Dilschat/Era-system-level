using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;

namespace Erasystemlevel.Tokenizer
{
    public class Tokenizer
    {
        readonly Regex numericRegex = new Regex("^(\\+|-)?\\d+$");
        readonly Regex identifierRegex = new Regex("\\b([A-Za-z][A-Za-z0-9_]*)\\b");
        private readonly Regex register = new Regex("\\bR([0-9]|[12][0-9]|3[01])\\b");
        private int lineNumber = 1;
        private int symbolNumber = 1;

        readonly HashSet<string> delimeters = new HashSet<string>(new List<string>
        {
            ";", ",", ".", "(", ")", "[", "]", ":>"
        });

        readonly HashSet<string> operators = new HashSet<string>(new List<string>
        {
            "+", "-", "*", "&", "|", "^", "?", "=", "<", ">", "/=", ":=",
            "+=", "-=", ">>=", "<<=", "|=", "&=", "^=", "<=", ">=", "?=",
            "<=>"
        });

        readonly HashSet<string> keywords = new HashSet<string>(new List<string>
        {
            "if", "else", "int", "short", "byte", "const", "routine", "do", "end",
            "start", "entry", "skip", "stop", "goto", "format", "for", "from", "to",
            "step", "while", "loop", "break", "then", "by", "trace", "data", "module", "code",
            "this"
        });

        readonly HashSet<string> whitespace = new HashSet<string>(new List<string>
        {
            "", "\n", "\r", " "
        });

        public readonly FileStream fileStream;
        public readonly StreamReader reader;


        public Tokenizer(string filePath)
        {
            fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            reader = new StreamReader(fileStream, Encoding.UTF8);
        }

        public Token Tokenize()
        {
            var currentToken = string.Empty;

            while (!reader.EndOfStream)
            {
                var next = Convert.ToChar(reader.Read());
                if (next.ToString().Equals("\n"))
                {
                    symbolNumber = 0;
                    lineNumber += 1;
                }

                if (!next.ToString().Equals("") && !next.ToString().Equals("\n"))
                {
                    symbolNumber += 1;
                }

                if (whitespace.Contains(next.ToString()))
                {
                    continue;
                }

                currentToken += next;


                if (currentToken.Equals("//"))
                {
                    currentToken = string.Empty;

                    while (!reader.EndOfStream)
                    {
                        next = Convert.ToChar(reader.Read());
                        if (next.Equals('\n'))
                        {
                            break;
                        }
                    }

                    continue;
                }

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
                    if (reader.EndOfStream || whitespace.Contains(Convert.ToString(reader.Peek())))
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
                    if (reader.EndOfStream || reader.Peek() == ' ' || Convert.ToString(reader.Peek()).Equals("\n"))
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

                while (numericRegex.IsMatch(currentToken + Convert.ToChar(reader.Peek())) && !reader.EndOfStream)
                {
                    currentToken = currentToken + Convert.ToChar(reader.Read());
                }

                return new Token(Token.TokenType.Number, currentToken);
            }

            reader.Close();
            return null;
        }


        public int GetLineNumber()
        {
            return lineNumber;
        }

        public int GetSymbolNumber()
        {
            return symbolNumber;
        }

        public (int, int) GetCurPosition()
        {
            return (lineNumber, symbolNumber);
        }
    }
}