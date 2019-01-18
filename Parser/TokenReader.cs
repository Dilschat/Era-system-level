using System.Collections.Generic;
using Erasystemlevel.Tokenizer;

namespace Erasystemlevel.Parser
{
    public class TokenReader
    {
        private Tokenizer.Tokenizer tokenizer;
        private Stack<Token> lookaheadBuffer = new Stack<Token>();
        private Stack<Token> savingBuffer = new Stack<Token>();
        private Stack<(int, int)> lookaheadNumberBuffer = new Stack<(int, int)>();
        private Stack<(int, int)> savingNumberBuffer = new Stack<(int, int)>();

        private Token lastToken;

        public TokenReader(Tokenizer.Tokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        public void saveReadTokens()
        {
            while (lookaheadBuffer.Count > 0)
            {
                savingBuffer.Push(lookaheadBuffer.Pop());
                savingNumberBuffer.Push(lookaheadNumberBuffer.Pop());
            }
        }

        public Token readNextToken()
        {
            if (savingBuffer.Count > 0)
            {
                lookaheadBuffer.Push(savingBuffer.Peek());
                lookaheadNumberBuffer.Push(savingNumberBuffer.Pop());

                return savingBuffer.Pop();
            }

            lastToken = tokenizer.Tokenize();
            lookaheadBuffer.Push(lastToken);
            lookaheadNumberBuffer.Push(tokenizer.GetCurPosition());

            return lastToken;
        }

        public void clear()
        {
            lookaheadBuffer.Clear();
            lookaheadNumberBuffer.Clear();
        }

        public bool isEmpty()
        {
            Token token;
            try
            {
                token = readNextToken();
            }
            catch (System.ObjectDisposedException)
            {
                return true;
            }

            if (token == null)
            {
                return true;
            }

            saveReadTokens();
            return false;
        }

        private int getCurrentBufferLength()
        {
            var tmp = 0;
            foreach (var (start, end) in lookaheadNumberBuffer)
            {
                tmp += end - start;
            }

            return tmp;
        }

        public Token getLastToken()
        {
            return lastToken;
        }

        public (int, int) getCurrentPosition()
        {
            saveReadTokens();

            return savingNumberBuffer.Pop();
        }
    }
}