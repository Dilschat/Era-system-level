﻿using System.Collections.Generic;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Parser
{
    public class TokenReader
    {
        private readonly Tokenizer.Tokenizer tokenizer;
        private readonly Stack<Token> lookaheadBuffer = new Stack<Token>();
        private readonly Stack<Token> savingBuffer = new Stack<Token>();
//        private Token lastToken;
        

        public TokenReader(Tokenizer.Tokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        public void SaveReadedTokens()
        {
            while (lookaheadBuffer.Count > 0)
            {
                savingBuffer.Push(lookaheadBuffer.Pop());
            }
        }

        public Token readNextToken()
        {
            if (savingBuffer.Count > 0)
            {
                lookaheadBuffer.Push(savingBuffer.Peek());
                return savingBuffer.Pop();
            }

            var nextToken = tokenizer.Tokenize();
            lookaheadBuffer.Push(nextToken);
            return nextToken;
        }

        public void clear()
        {
            lookaheadBuffer.Clear();
        }
    }
}