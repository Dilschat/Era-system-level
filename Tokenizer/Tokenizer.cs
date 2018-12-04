using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Erasystemlevel.Tokenizer
{
    public class Tokenizer
    {
        Regex numericRegex = new Regex("^(\\+|-)?\\d+$");
        Regex identifierRegex = new Regex("\\b([A-Za-z][A-Za-z0-9_]*)\\b");
        public Regex register = new Regex("\\bR([0-9]|[12][0-9]|3[01])\\b");
        HashSet<string> delimeters = new HashSet<string>(new List<string>()
        {
            ";", ",", ".", "(", ")", "[", "]", "//"
        });
        HashSet<string> operators = new HashSet<string>(new List<string>()
        {
            "+", "-", "*", "&", "|", "^", "?", "=", "<", ">", "/=", ":=",
            "+=", "-=", ">>=", "<<=", "|=", "&=", "^=", "<=", ">=", "?=",
            "<=>"
            
        });

        HashSet<string> keywords = new HashSet<string>(new List<string>()
        {
            "if", "else", "int", "short", "byte", "const", "routine", "do", "end",
            "start", "entry", "skip", "stop", "goto", "format", "for", "from", "to",
            "step", "while", "loop", "break", "then", "by", "trace", "data", "module", "code",
            "this"
        });
        private StreamReader reader;


        public Tokenizer(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open,
                FileAccess.Read);
            this.reader = new StreamReader(fs, Encoding.UTF8);
        }

        public Token Tokenize()
        {
            string currentToken = String.Empty;

            while (!this.reader.EndOfStream)
            {
                
                    Char next = Convert.ToChar(this.reader.Read());
                    if (next.ToString() == "" || next.ToString() =="\n" || next.ToString() == " "){
                        continue;
                    }
                    currentToken += next;
                    //Console.WriteLine(currentToken);

                    if (delimeters.Contains(currentToken))
                    {
                        return new Token(Token.TokenType.Delimiter, currentToken); 
                    }
                    if(register.IsMatch(currentToken)){
                    char nextChar = Convert.ToChar(this.reader.Peek());

                    if (!Char.IsDigit(nextChar) && !Char.IsLetter(nextChar) && nextChar.ToString() != "_"){
                        return new Token(Token.TokenType.Register, currentToken);
                    }
                    }
                    if (operators.Contains(currentToken))
                    {
                        while (operators.Contains(currentToken+ Convert.ToChar(this.reader.Peek()))){
                            currentToken = currentToken + Convert.ToChar(this.reader.Read());  
                        }
                        return new Token(Token.TokenType.Operator, currentToken);

                    }
                    if (keywords.Contains(currentToken)){
                        if (this.reader.EndOfStream || Convert.ToString(this.reader.Peek()).Equals(" ") || Convert.ToString(this.reader.Peek()).Equals("/n"))
                        {
                            return new Token(Token.TokenType.Keyword, currentToken);
                        }
                        char nextChar = Convert.ToChar(this.reader.Peek());
                        if (!Char.IsDigit(nextChar)&&!Char.IsLetter(nextChar)&& nextChar.ToString()!="_"){
                            return new Token(Token.TokenType.Keyword, currentToken);
                        }
                    }
                    if(identifierRegex.IsMatch(currentToken)){
                        if(this.reader.EndOfStream || Convert.ToString(this.reader.Peek()).Equals(" ") || Convert.ToString(this.reader.Peek()).Equals("/n"))
                        {
                            return new Token(Token.TokenType.Identifier, currentToken);
                        }
                        char nextChar = Convert.ToChar(this.reader.Peek());
                        if ((Char.IsDigit(nextChar) || Char.IsLetter(nextChar)) && nextChar != ' ')
                        {
                            continue;
                        }else{
                            return new Token(Token.TokenType.Identifier, currentToken);
                        }
                    }
                    if (numericRegex.IsMatch(currentToken))
                    {
                        if (reader.EndOfStream)
                        {
                            return new Token(Token.TokenType.Number, currentToken);
                        }
                        while (numericRegex.IsMatch(currentToken+Convert.ToChar(this.reader.Peek())) && !this.reader.EndOfStream){
                            currentToken = currentToken + Convert.ToChar(this.reader.Read());
                        }
                       return new Token(Token.TokenType.Number, currentToken);

                    }


            }
            this.reader.Close();
            return null;
        }






    }
}
