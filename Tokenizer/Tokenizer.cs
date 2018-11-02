﻿using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Erasystemlevel.Tokenizer
{
    public class Tokenizer
    {
        Regex numericRegex = new Regex("1");
        Regex identifierRegex = new Regex("f");
        HashSet<string> delimeters = new HashSet<string>(new List<string>() { ";", "," });
        HashSet<string> operators = new HashSet<string>(new List<string>() { ">", "<" });
        HashSet<string> keywords = new HashSet<string>(new List<string>() { "if", "else" });
        StreamReader reader;


        public Tokenizer(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open,
                FileAccess.Read);
            this.reader = new StreamReader(fs, Encoding.UTF8);
        }

        public LinkedList<Token> Tokenize()
        {
            LinkedList<Token> list = new LinkedList<Token>();

            while (!this.reader.EndOfStream)
            {
                string input = this.reader.ReadLine();
                char[] chars = input.ToCharArray();
                string currentToken = String.Empty;
                for (int i = 0; i < chars.Length; i++)
                {
                    if(chars[i] == ' ' || chars[i] =='\n'){
                        continue;
                    }
                    currentToken += chars[i];

                    if (delimeters.Contains(currentToken))
                    {
                        list.AddLast(new Token(Token.TokenType.Delimiter, currentToken)) ;
                        currentToken = "";
                        continue;

                    }
                    if (operators.Contains(currentToken))
                    {
                        String extendedToken = currentToken;
                        while (operators.Contains(extendedToken)){
                            currentToken = extendedToken;
                            i++;
                            if (i == chars.Length)
                            {
                                break;
                            }
                            extendedToken = currentToken + chars[i];
                        }
                        i--;
                        list.AddLast(new Token(Token.TokenType.Operator, currentToken));
                        currentToken = "";
                        continue;


                    }
                    if (keywords.Contains(currentToken)){
                        if (i + 1 == chars.Length)
                        {
                            list.AddLast(new Token(Token.TokenType.Keyword, currentToken));
                            currentToken = "";
                            continue;
                        }
                        char nextChar = chars[i + 1];
                        if (!Char.IsDigit(nextChar)&&!Char.IsLetter(nextChar)&&nextChar!='_'){
                            list.AddLast(new Token(Token.TokenType.Keyword, currentToken));
                            currentToken = "";
                            continue;
                        }
                    }
                    if(identifierRegex.IsMatch(currentToken)){
                        if(i+1 == chars.Length){
                            list.AddLast(new Token(Token.TokenType.Identifier, currentToken));
                            currentToken = "";
                            continue;
                        }
                        char nextChar = chars[i + 1];
                        if (Char.IsDigit(nextChar) && Char.IsLetter(nextChar) && nextChar != '_')
                        {
                            continue;
                        }else{
                            list.AddLast(new Token(Token.TokenType.Identifier, currentToken));
                            currentToken = "";
                            continue;
                        }
                    }
                    if (numericRegex.IsMatch(currentToken))
                    {
                        String extendedToken = currentToken;
                        while (numericRegex.IsMatch(extendedToken)){
                            currentToken = extendedToken;
                            i++;
                            if(i == chars.Length){
                                break;
                            }
                            extendedToken = currentToken + chars[i];
                        }
                        i--;
                        list.AddLast(new Token(Token.TokenType.Number, currentToken));
                        currentToken = "";
                        continue;
                    }
                }

            }
            return list;
        }






    }
}